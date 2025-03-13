export interface SearchedFiles {
  files: DownloadFile[];
  searchQuery: string;
}

export interface DownloadFile {
  fileName: string;
  fileContent: string;
  countOfOccurrences: number;
}
