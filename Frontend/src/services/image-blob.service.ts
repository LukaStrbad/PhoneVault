import { Injectable } from '@angular/core';
import { environment } from "../environments/environment";
import { HttpClient } from "@angular/common/http";
import { firstValueFrom } from "rxjs";

const url = `${environment.apiUrl}/ImageBlob`;

@Injectable({
  providedIn: 'root'
})
export class ImageBlobService {

  constructor(
    private http: HttpClient
  ) { }

  async addImages(files: FileList) {
    const formData = new FormData();
    for (let i = 0; i < files.length; i++) {
      formData.append('file', files[i]);
    }
    const names = await firstValueFrom(this.http.post<string[]>(`${url}`, formData));
    return names.map(name => `${url}/${name}`);
  }
}
