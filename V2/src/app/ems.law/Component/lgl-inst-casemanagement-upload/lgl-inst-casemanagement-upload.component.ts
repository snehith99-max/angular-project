
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-lgl-inst-casemanagement-upload',
  templateUrl: './lgl-inst-casemanagement-upload.component.html',
  styleUrls: ['./lgl-inst-casemanagement-upload.component.scss']
})
export class LglInstCasemanagementUploadComponent  {
  selectedFiles!: FileList;
  fileArray: File[] = [];
  formData = new FormData();
  currentPath: any;
  case_gid: any;

  constructor(private route: Router,
    private router: ActivatedRoute,
    public service: SocketService,
    private ToastrService: ToastrService,
    public NgxSpinnerService: NgxSpinnerService,
  ) { }

  ngOnInit(): void {
    debugger
    //this.currentPath = event.urlAfterRedirects;
    this.NgxSpinnerService.show()
    const case_gid = this.router.snapshot.paramMap.get('case_gid1');
    this.case_gid = case_gid;

    const key = 'storyboarderp';
    const deencry = AES.decrypt(this.case_gid, key).toString(enc.Utf8);
    this.formData.append('case_gid', deencry);
    this.NgxSpinnerService.hide()
  }

  onFileSelected(event: any): void {
    this.selectedFiles = event.target.files;
    const newFiles = Array.from(this.selectedFiles);
    newFiles.forEach(newFile => {
      if (!this.fileArray.some(file => file.name === newFile.name && file.size === newFile.size)) {
        this.fileArray.push(newFile);
        this.formData.append('files[]', newFile);
      }
    });
  }

  removeFile(index: number): void {
    this.fileArray.splice(index, 1);
  }

  back() {
    this.route.navigate(['/legal/LglIstCaseManagement'])
  }
  
  sumbit() {
    debugger
    this.NgxSpinnerService.show();
    var api = 'CaseManagement/PostCaseDoc';
    this.service.postparams(api, this.formData).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else{
        this.route.navigate(['/legal/LglIstCaseManagement']);
        this.ToastrService.success(result.message)
      }
    });
    this.NgxSpinnerService.hide();
  }
}
