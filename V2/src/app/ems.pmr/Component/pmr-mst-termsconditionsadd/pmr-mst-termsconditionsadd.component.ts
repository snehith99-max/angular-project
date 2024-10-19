import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgxSpinnerService } from 'ngx-spinner';
interface Template{
  template_name: string;
  payment_terms: string;
  template_content: string;
}
@Component({
  selector: 'app-pmr-mst-termsconditionsadd',
  templateUrl: './pmr-mst-termsconditionsadd.component.html',
})
export class PmrMstTermsconditionsaddComponent {
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '25rem',
    minHeight: '5rem',
    width: '1050px',
    placeholder: 'Enter text here...',
    translate: 'no',
    
   
  };
  reactiveForm!: FormGroup | any;
  template_list: any[] = [];
  Gettemplate_list: any[] = [];
  responsedata: any;
  mdlBranchName :any;
  template!: Template
  constructor(private formBuilder: FormBuilder,private route: Router,public NgxSpinnerService:NgxSpinnerService, private ToastrService: ToastrService, public service: SocketService) {
    this.template = {} as Template;

  }

  ngOnInit(): void {
    this.GetTermsConditionsSummary();
    this.reactiveForm = new FormGroup({
      template_name:  new FormControl('',[Validators.required]),
      payment_terms:  new FormControl('',[Validators.required]),
      template_content: new FormControl('',[Validators.required]),
    });
  }
  get template_name() {
    return this.reactiveForm.get('template_name');
  }

  get payment_terms() {
    return this.reactiveForm.get('payment_terms');
  }
  GetTermsConditionsSummary(){

    var url = 'PmrMstTermsConditions/GetTermsConditionsSummary'
    this.NgxSpinnerService.show();
 
    this.service.get(url).subscribe((result: any) => {
 
      this.responsedata = result;
 
      this.Gettemplate_list = this.responsedata.Gettemplate_list;
 
      //console.log(this.entity_list)
 
      setTimeout(() => {
 
        $('#Gettemplate_list').DataTable();
 
      }, 1);
    });
    this.NgxSpinnerService.hide();
  }
  
  
  public onsubmit(): void{
    debugger
    if (this.reactiveForm.value.template_name != null && this.reactiveForm.value.template_name !='' && this.reactiveForm.value.payment_terms != '') {
    var api = 'PmrMstTermsConditions/PostTermsConditions';
    this.NgxSpinnerService.show();
    this.service.post(api, this.reactiveForm.value).subscribe((result: any) => {
      //this.responsedata = result;
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.route.navigate(['/pmr/PmrMstTermsconditionssummary']);
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    });
  }
     
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    
  }
  
}
