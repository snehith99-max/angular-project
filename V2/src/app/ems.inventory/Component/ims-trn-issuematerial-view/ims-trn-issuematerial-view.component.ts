// import { Component } from '@angular/core';
// import { ToastrService } from 'ngx-toastr';
// import { AES, enc } from 'crypto-js';
// import { ActivatedRoute, Router } from '@angular/router';
// import { FormBuilder } from '@angular/forms';
// import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Component, ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, map, share, timer } from 'rxjs';
@Component({
  selector: 'app-ims-trn-issuematerial-view',
  templateUrl: './ims-trn-issuematerial-view.component.html',
  styleUrls: ['./ims-trn-issuematerial-view.component.scss']
})
export class ImsTrnIssuematerialViewComponent {


  config: AngularEditorConfig = {
    editable: false,
    spellcheck: false,
    height: '25rem',
    minHeight: '5rem',
    width: '1000px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };
  Viewissuematerial_list: any[] = [];
  Viewissuematerialsummary_list: any []=[];
  customer: any;
  responsedata: any;
  reference_gid:any;
  lspage:any;


  // constructor(private formBuilder: FormBuilder, private route: Router, private router: ActivatedRoute, public service: SocketService) { }

  // ngOnInit(): void {
  //   debugger;
  //   const reference_gid = this.router.snapshot.paramMap.get('reference_gid');
  //   this.customer = reference_gid;
  //   const secretKey = 'storyboarderp';
  //   const deencryptedParam = AES.decrypt(this.customer,secretKey).toString(enc.Utf8);
  //   console.log(deencryptedParam)
  //   this.GetViewIssueMaterial(deencryptedParam);
  // }

  constructor(private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private route: Router,
    private router: ActivatedRoute,
    private ToastrService: ToastrService,
    private NgxSpinnerService: NgxSpinnerService)  {
     }
     ngOnInit(): void {
      debugger;
        this.reference_gid = this.router.snapshot.paramMap.get('materialissued_gid');
        const secretKey = 'storyboarderp';
        const deencryptedParam = AES.decrypt(this.reference_gid, secretKey).toString(enc.Utf8);
        this.lspage = this.router.snapshot.paramMap.get('lspage');
        this.lspage = this.lspage;
        const lspage = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
        this.lspage = lspage;
        console.log(deencryptedParam+"reference_gid");
        this.GetViewIssueMaterial(deencryptedParam);
     }

  GetViewIssueMaterial(reference_gid: any) {
    debugger;

    var url = 'ImsTrnIssueMaterial/GetViewIssueMaterialSummary'
    let params = {
      reference_gid: reference_gid
    }
    this.service.getparams(url, params).subscribe((result: any) => {
      this.responsedata = result;
      this.Viewissuematerialsummary_list = result.Viewissuematerialsummary_list;
    });
    var url = 'ImsTrnIssueMaterial/GetViewIssueMaterial'
    let param = {
      reference_gid: reference_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Viewissuematerial_list = result.Viewissuematerial_list;
    });
  }

  back() {
    if(this.lspage == 'Inventory'){
      this.route.navigate(['/ims/ImsRptMaterialIssueReport']);
    }
    else{
      this.route.navigate(['/ims/ImsTrnIssuematerialSummary']);
    }

  }



}
