import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';
interface ISmsCampaign {
  campaign_title: string;
  campaign_message: string;
  template_id: string;
  campaign_titleedit: string;
  campaign_messageedit: string;
}
@Component({
  selector: 'app-crm-smm-smscampaign',
  templateUrl: './crm-smm-smscampaign.component.html',
  styleUrls: ['./crm-smm-smscampaign.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmSmmSmscampaignComponent {
  buttonDisabled: boolean;
  showOptionsDivId: any;
  responsedata: any;
  smscampaignlist: any[] = [];
  parameterValue1: any;
  parameterValue: any;
  parameterValue2: any;
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '12rem',
    minHeight: '5rem',
    width: '1000px',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',

  };
  myForm: any;
  maxChar = 450;
  maxChars: any;
  maxCharsForDescription: any;
  reactiveForm!: FormGroup;
  reactiveFormView!: FormGroup;
  reactiveFormEdit!: FormGroup;
  smscampaign!: ISmsCampaign;
  smscampaigncount_list: any;
  totalcampaign_send: any;
  campaign_count: any;
  current_month: any;
  current_year: any;
  crt_mth: any;
  crt_yr: any;

  constructor(private formBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private route: Router, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.smscampaign = {} as ISmsCampaign;
    this.buttonDisabled = this.isButtonDisabled();

  }


  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmsCamapaign();
    this.reactiveForm = new FormGroup({

      campaign_title: new FormControl(this.smscampaign.campaign_title, [

        Validators.required,


      ]),

      campaign_message: new FormControl(this.smscampaign.campaign_message, [
        Validators.required,
      ]),
    });
    this.reactiveFormView = new FormGroup({
      campaign_title: new FormControl(this.smscampaign.campaign_title, [
        Validators.required,
      ]),
      campaign_message: new FormControl(this.smscampaign.campaign_message, [
        Validators.required,
      ]),
    });
    this.reactiveFormEdit = new FormGroup({
      campaign_titleedit: new FormControl(this.smscampaign.campaign_titleedit, [
        Validators.required,
      ]),
      campaign_messageedit: new FormControl(this.smscampaign.campaign_messageedit, [
        Validators.required,
      ]),
      template_id: new FormControl(),
    });
    this.buttonDisabled = this.isButtonDisabled();

  }
  public smslog(params: any): void {
    const secretKey = 'storyboarderp';
    const template_id = AES.encrypt(params.template_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmSmslog', template_id])
  }
  isButtonDisabled(): boolean {
    const currentTime = new Date();
    const hours = currentTime.getHours();
    return hours < 10 || hours >= 21;
  }
  GetSmsCamapaign() {

    var url1 = 'SmsCampaign/GetSmsCampaign'
    this.service.get(url1).subscribe((result: any) => {
      this.responsedata = result;
      this.smscampaignlist = this.responsedata.smscampaign_list;

      //  console.log(this.shopify_customerlist)
      setTimeout(() => {
        $('#smscampaignlist').DataTable();
      }, 1);

    });
    var url3 = 'SmsCampaign/GetSmsCampaignCount'
    this.service.get(url3).subscribe((result: any) => {
      this.responsedata = result;
      this.campaign_count = this.responsedata.campaign_count;
      this.totalcampaign_send = this.responsedata.totalcampaign_send;
      this.current_month = this.responsedata.current_month;
      this.current_year = this.responsedata.current_year;
      this.crt_mth = this.responsedata.crt_mth;
      this.crt_yr = this.responsedata.crt_yr;

    });
  }
  toggleOptions(template_id: any) {
    if (this.showOptionsDivId === template_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = template_id;
    }
  }
  get campaign_title() {
    return this.reactiveForm.get('campaign_title')!;
  }
  get campaign_message() {
    return this.reactiveForm.get('campaign_message')!;
  }
  get campaign_titleedit() {
    return this.reactiveFormEdit.get('campaign_titleedit')!;
  }
  get campaign_messageedit() {
    return this.reactiveFormEdit.get('campaign_messageedit')!;
  }

  public onsubmit(): void {
    if (this.reactiveForm.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'SmsCampaign/PostSmsCampaign'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetSmsCamapaign();
          this.reactiveForm.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetSmsCamapaign();
          this.reactiveForm.reset();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }
  public onupdate(): void {
    if (this.reactiveFormEdit.status === 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'SmsCampaign/UpdateSmsCampaign'
      this.service.post(url, this.reactiveFormEdit.value).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.GetSmsCamapaign();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.GetSmsCamapaign();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }


  }
  onclose() {
    this.reactiveForm.reset();
  }
  openModalview(parameter: any) {
    this.parameterValue1 = parameter
    this.reactiveFormView.get("campaign_title")?.setValue(this.parameterValue1.campagin_title);
    this.reactiveFormView.get("campaign_message")?.setValue(this.parameterValue1.campagin_message);

  }
  openModaledit(parameter: any) {
    this.parameterValue2 = parameter
    this.reactiveFormEdit.get("campaign_titleedit")?.setValue(this.parameterValue2.campagin_title);
    this.reactiveFormEdit.get("campaign_messageedit")?.setValue(this.parameterValue2.campagin_message);
    this.reactiveFormEdit.get("template_id")?.setValue(this.parameterValue2.template_id);
  }
  openModaldelete(parameter: any) {
    this.parameterValue = parameter
  }
  ondelete() {
    //console.log(this.parameterValue);
    this.NgxSpinnerService.show();
    var url = 'SmsCampaign/DeleteSmsCampaign'
    let param = {
      template_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message)
        this.reactiveForm.reset();

      }
      this.GetSmsCamapaign();



    });
  }
 
  public onsend(params: any): void {
    debugger
    const secretKey = 'storyboarderp';
    //console.log(params)
    const template_id = AES.encrypt(params.template_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmSmscampaignsend', template_id])

  }
}
