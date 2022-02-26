export class FileUpload {
    Key!: string;
    Name!: string;
    Url!: string;
    File: File;
    constructor(file: File) {
      this.File = file;
    }
  }