import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {debounceTime, distinctUntilChanged, Observable, Subject, switchMap, throwError} from "rxjs";
import {DownloadFile, SearchedFiles} from "./models/searchedFiles";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Search-Enron';
  private packages$!: Observable<any>;
  private searchTerm$ = new Subject<string>();
  searchResult: SearchedFiles = {files: [], searchQuery: ""};
  status: "initial" | "uploading" | "success" | "fail" = "initial";
  files: File[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
    this.packages$ = this.searchTerm$.pipe(
      debounceTime(5000),
      distinctUntilChanged(),
      switchMap(async (searchQuery) => this.search(searchQuery))
    )
  }

  search(searchQuery: string) {
    var result = this.http.get<SearchedFiles>("http://localhost:5109/api/search?searchQuery=" + searchQuery);

    result.subscribe({
      next: result => {
        this.searchResult = result;
      }
    })
    console.log(this.searchResult);
  }

  onChange(event: any) {
    const files = event.target.files;

    if (files.length) {
      this.status = "initial";
      this.files = files;
    }
  }

  onUpload() {
    if (this.files.length) {
      const formData = new FormData();

      [...this.files].forEach((file) => {
        formData.append("files", file, file.name)
      });

      const upload$ = this.http.post("http://localhost:5110/api/cleaner/clean", formData);

      this.status = "uploading";

      upload$.subscribe({
        next: () => {
          this.status = "success";
        },
        error: (error: any) => {
          this.status = "fail";
          console.log(error);
          return throwError(() => error);
        }
      })
    }
  }

  onClear() {
    this.status = "initial";
    this.files = [];
  }

  getValue(event: Event) {
    return (event.target as HTMLInputElement).value;
  }

  onDownload(result: DownloadFile) {
    const byteCharacters = atob(result.fileContent);
    const byteArray = [];

    for (let offset = 0; offset < byteCharacters.length; offset += 1024) {
      const slice = byteCharacters.slice(offset, offset + 1024);
      const byteNumbers = new Array(slice.length);

      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }

      byteArray.push(new Uint8Array(byteNumbers));
    }

    const blob = new Blob(byteArray, { type: "application/octet-stream" });

    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.href = url;
    link.download = result.fileName || "download_file";
    link.click();

    URL.revokeObjectURL(url);
  }
}
