import {Component} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {throwError} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Search-Enron';
  searchTerm: string = '';
  status: "initial" | "uploading" | "success" | "fail" = "initial";
  files: File[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit() {
  }

  search() {

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
}
