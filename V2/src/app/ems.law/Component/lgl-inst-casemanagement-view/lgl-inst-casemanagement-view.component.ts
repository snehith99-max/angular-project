
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-lgl-inst-casemanagement-view',
  templateUrl: './lgl-inst-casemanagement-view.component.html',
  styleUrls: ['./lgl-inst-casemanagement-view.component.scss']
})
export class LglInstCasemanagementViewComponent {
  groupedData: any;
  case_gid: any;
  Getviewsummary_list: any[] = [];
  files: Array<{name: string, size: number, blobUrl: string}> = [];
  filesWithId: { file: File; doc_gid: string; doc_attpath: string }[] = [];
  pdfFiles : { url: string, name: string }[] = [];
  GetDocument_list : any  []= [];
  doc_filepath: any;
  responsedata: any;
  GetViewDocument_list : any;

  constructor(private route: Router,
    private router: ActivatedRoute,
    public service: SocketService,private sanitizer: DomSanitizer,
    public NgxSpinnerService: NgxSpinnerService
  ) { }

  ngOnInit(): void {
    debugger
    const case_gid1 = this.router.snapshot.paramMap.get('case_gid1')
    this.case_gid = case_gid1;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.case_gid, secretKey).toString(enc.Utf8);
    this.GetCaseSummaryView(deencryptedParam);
    this.GetDocumentSummary(deencryptedParam);
  }


  GetCaseSummaryView(case_gid: any) {
    debugger
    this.NgxSpinnerService.show();
    let param = { case_gid };
    var api = 'CaseManagement/GetViewSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      this.Getviewsummary_list = result.GetViewSummaryCase_list;
    });
    this.NgxSpinnerService.hide();
  }
//   GetDocumentSummary(case_gid:string) {
//     debugger
//     var params = {
//       case_gid : case_gid 
//     }
//     var url = 'CaseManagement/GetDocumentupload'
//     this.service.getparams(url,params).subscribe((result: any) => {
//       this.responsedata = result;
//       this.GetViewDocument_list = this.responsedata.GetViewDocument_list;
//     });
//  }

GetDocumentSummary(case_gid: string) {
  debugger;
  var params = {
    case_gid: case_gid
  };
  var url = 'CaseManagement/GetDocumentupload';
  this.service.getparams(url, params).subscribe((result: any) => {
    this.responsedata = result;
    this.groupedData = this.groupDataByCaseStage(result.GetViewDocument_list);
  });
}
groupDataByCaseStage(data: any[]) {
  return data.reduce((acc, curr) => {
    (acc[curr.casestage_name] = acc[curr.casestage_name] || []).push(curr);
    return acc;
  }, {});
}
groupedDataKeys(): string[] {
  if (!this.groupedData) {
    return [];
  }
  return Object.keys(this.groupedData);
}

  downloadImage(filepath: string , doc_name: string, doc_extension: string) {
    let link = document.createElement('a');
    link.href = environment.URL_FILEPATH + encodeURIComponent(filepath) + encodeURIComponent(doc_extension);
    link.download = doc_name + doc_extension; 
    document.body.appendChild(link); 
    link.click(); 
    document.body.removeChild(link); 
}

viewFile(filepath: string, doc_name: string, doc_extension: string) {
  debugger
  let link = document.createElement('a');
  link.href = environment.URL_FILEPATH + encodeURIComponent(filepath) + encodeURIComponent(doc_extension);
  link.target = '_blank'; // Open in a new tab
  document.body.appendChild(link); 
  link.click(); 
  document.body.removeChild(link); 
}

}
