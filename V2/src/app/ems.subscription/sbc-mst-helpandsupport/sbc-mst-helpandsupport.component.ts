import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'app-sbc-mst-helpandsupport',
  templateUrl: './sbc-mst-helpandsupport.component.html',
  styleUrls: ['./sbc-mst-helpandsupport.component.scss']
})
export class SbcMstHelpandsupportComponent {
  reactiveform: FormGroup | any;
  submodule_list: any;
  module_list: any;
  selectedmodulename: any;
  selectedscreenname:any;
  support_list:any[]=[];
  responsedata:any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '0rem',
    // width: '1080px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
  };

  constructor(private SocketService: SocketService, private NgxSpinnerService: NgxSpinnerService, public service: SocketService, private ToastrService: ToastrService, private FormBuilder: FormBuilder) {

    this.reactiveform = new FormGroup({
      module_name: new FormControl('',Validators.required),
      module_gid: new FormControl(),
      Module_name: new FormControl('',Validators.required),
      Module_gid: new FormControl(),
      company_name: new FormControl('',Validators.required),
      Description: new FormControl('',Validators.required),
      mail_id: new FormControl('',Validators.required),
      // ticket_id:new FormControl('',Validators.required),
      contact_number:new FormControl('',Validators.required),
    })
  }
  ngOnInit(): void {
    this.GetModuleNameSummary();
    this.  GetHelpandSupportSummary();
  }
  GetModuleNameSummary() {
    var api = 'CampaignService/GetModuleNameSummary'
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.module_list = this.responsedata.module_list;
      this.reactiveform.get("module_gid")?.setValue(this.module_list.module_gid);
      this.reactiveform.get("module_name")?.setValue(this.module_list.module_name);
    });
  }
  OnChangeScreenNameSummary() {
    let module = this.reactiveform.get("module_name")?.value
    if (module != '' || module != null) {
      let param = {
        module_gid: module
      }
      var api = 'CampaignService/GetScreenNameSummary'
      this.service.getparams(api, param).subscribe((result: any) => {
        this.responsedata = result;
        this.submodule_list = this.responsedata.submodule_list;
        this.reactiveform.get("Module_name")?.setValue(this.submodule_list.Module_name);
        this.reactiveform.get("Module_gid")?.setValue(this.submodule_list.Module_gid);
      });
    }
    else {
      this.ToastrService.warning('select module name')
    }
  }
  onsubmit(){
    this.NgxSpinnerService.show();
    var url = 'HelpandSupport/PostHelpandSupport';
    this.SocketService.postparams(url, this.reactiveform.value).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.reactiveform.reset();
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  }
  GetHelpandSupportSummary() {
    var url = 'HelpandSupport/GetHelpandSupportSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#support_list').DataTable().destroy();
      this.responsedata = result;
      this.support_list = this.responsedata.SupportLists;
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#support_list').DataTable();
      }, 1);
    })
  }
}
