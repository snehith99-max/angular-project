import { HttpClient, HttpEvent, HttpHandler, HttpHeaders, HttpParams, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

interface DocumentListResponse {
  GetViewDoc_list: Document[];
}

interface Document {
  doc_name: string;
  doc_filepath: string;
}

@Injectable({
  providedIn: 'root'
})
export class SocketService {
  sortColumnKey: string = ''; // track the current column being sorted
  sortDirection: string = 'asc';
    

  constructor(private http: HttpClient) {
  }
  get(api: string) {
    return this.http.get(api);

  }
  getdtl(api: string) {
    var user_gid = localStorage.getItem('user_gid');
    return this.http.get<any>(api + '?user_gid=' + user_gid);
  }
  getparams(api: string, params: any) {
    return this.http.get(api, { params: params });
  }

  getfileparams(api: string, params: any): Observable<DocumentListResponse> {

    return this.http.get<DocumentListResponse>(api, { params });
  }

  postparams(api: string, params: any) {
    var user_gid = localStorage.getItem('user_gid');
    return this.http.post<any>(api + '?user_gid=' + user_gid, params);
  }
  download(api: string, params: any) {
    return this.http.get(api, { params: params });
  }


  getid(url: string, params: any) {
    return this.http.get<any>(url + '?params_gid=' + params);
  }

  //////Snehith code Start//////////////////
  postfile(api: string, params: any) {


    return this.http.post(api, params);
  }

  //////Snehith code End//////////////////
  post(api: string, params: any) {
    return this.http.post(api, params);
  }
  generateexcel(api: string) {
    return this.http.get(api);

  }
  downloadimage(api: string, params: any) {

    return this.http.get(api, { params: params });
  }
  downloadFile(params: any) {
    return this.http.post('AzureStorage/DownloadDocument', params);
  }
  downloadfile(path: any, file_name: any) {
    var params = {
      file_path: path,
      file_name: file_name
    }
    return this.http.post('AzureStorage/DownloadDocument', params);
  }


  filedownload(data: any) {
    const file_type = this.createBlobFromExtension(data.format);
    const blob = new Blob([data.file], { type: file_type });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = data.name; // Specify the desired file name and extension.
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);

  }
  filedownload1(data: any) {
    debugger
    const file_type = this.createBlobFromExtension(data.format);
    const base64Content = data.file;
    // Convert base64 to binary
    const binaryContent = atob(base64Content);
    // Create a Uint8Array from the binary content
    const uint8Array = new Uint8Array(binaryContent.length);
    for (let i = 0; i < binaryContent.length; i++) {
      uint8Array[i] = binaryContent.charCodeAt(i);
    }
    const blob = new Blob([uint8Array], { type: file_type });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = data.name; // Specify the desired file name and extension.
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
  }
  fileviewer(data: any) {
    const file_type = this.createBlobFromExtension(data.format);
    const base64Content = data.file;
 
    // Convert base64 to binary
    const binaryContent = atob(base64Content);
 
    // Create a Uint8Array from the binary content
    const uint8Array = new Uint8Array(binaryContent.length);
    for (let i = 0; i < binaryContent.length; i++) {
      uint8Array[i] = binaryContent.charCodeAt(i);
    }
    const blob = new Blob([uint8Array], { type: file_type });
    const url = window.URL.createObjectURL(blob);
    const newTab = window.open(url, '_blank');
 
    if (newTab) {
      newTab.focus();
    }
    setTimeout(() => {
      URL.revokeObjectURL(url);
    }, 60000);
  }

  filetxtdownload(data: any, file_name: string): void {

    const text = data.map((item: any) => JSON.stringify(item)).join('\n');
    const blob = new Blob([text], { type: 'text/plain' });

    const anchor = document.createElement('a');
    anchor.download = file_name;
    anchor.href = window.URL.createObjectURL(blob);
    anchor.click();

    window.URL.revokeObjectURL(anchor.href);
  }

  createBlobFromExtension(file_extension: any) {
    debugger
    const mimeTypes: { [key: string]: string } = {
      txt: 'text/plain',
      html: 'text/html',
      css: 'text/css',
      js: 'application/javascript',
      json: 'application/json',
      xml: 'application/xml',
      pdf: 'application/pdf',
      doc: 'application/msword',
      docx: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
      xls: 'application/vnd.ms-excel',
      xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      ppt: 'application/vnd.ms-powerpoint',
      pptx: 'application/vnd.openxmlformats-officedocument.presentationml.presentation',
      jpeg: 'image/jpeg',
      jpg: 'image/jpeg',
      png: 'image/png',
      gif: 'image/gif',
      bmp: 'image/bmp',
      tiff: 'image/tiff',
      svg: 'image/svg+xml',
      mp3: 'audio/mpeg',
      wav: 'audio/wav',
      mp4: 'video/mp4',
      mov: 'video/quicktime',
      avi: 'video/x-msvideo',
      zip: 'application/zip',
      rar: 'application/x-rar-compressed',
      // Add more mappings as needed for other extensions
    };
    const minetype = mimeTypes[file_extension.toLocaleLowerCase()];
    return minetype;
  }

  fileattach(data: any, filename: string): Blob {
    const base64Content = data.file; 
    const binaryContent = atob(base64Content); 
    const uint8Array = new Uint8Array(binaryContent.length);

    for (let i = 0; i < binaryContent.length; i++) {
      uint8Array[i] = binaryContent.charCodeAt(i);
    }
    const blob = new Blob([uint8Array], { type: 'application/pdf' });
    return blob; 
  }

  sortColumn(columnKey: string): void {
    if (this.sortColumnKey === columnKey) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumnKey = columnKey;
      this.sortDirection = 'asc';
    }
  }

  getSortIconClass(columnKey: string) {
    return {
      'fa-sort-asc': this.sortColumnKey === columnKey && this.sortDirection === 'asc',
      'fa-sort-desc': this.sortColumnKey === columnKey && this.sortDirection === 'desc',
      'fa-sort': this.sortColumnKey !== columnKey
    };
  }

  

  fileattachs(result: any, fileName: string): Blob {
    debugger
    const minetype = this.createBlobFromExtension(result.format);
    const byteCharacters = atob(result.doc_filepath);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type:  minetype });
  }
  
  msASN_PDF(base64: string, file_name: string): void {

    const text = base64;
    const blob = new Blob([text], { type: 'text/plain' });

    const anchor = document.createElement('a');
    anchor.download = file_name;
    anchor.href = window.URL.createObjectURL(blob);
    anchor.click();

    window.URL.revokeObjectURL(anchor.href);
  }
}