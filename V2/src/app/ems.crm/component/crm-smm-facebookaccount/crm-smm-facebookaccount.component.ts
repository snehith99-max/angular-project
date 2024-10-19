import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import {
  FormBuilder, FormControl, FormGroup, Validators, ValidationErrors,
  AbstractControl,
  ValidatorFn
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import flatpickr from 'flatpickr';
import { Options } from 'flatpickr/dist/types/options'; import { AES } from 'crypto-js';
interface IFacebook {
  image_caption: string;
  file_path: string;
  video_caption: string;
  image_schedulercaption: string;
  schedule_at: string;

}
interface IFacebookpost {
  caption: string;
  file_path: string;
  image_schedulercaption: string;
  schedule_at: string;
  page_name: string
}
interface IPlatform {
  image_caption: string;
  file_path: string;
  page_name: string,
  username: string;
  mention: string;
}
@Component({
  selector: 'app-crm-smm-facebookaccount',
  templateUrl: './crm-smm-facebookaccount.component.html',
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
export class CrmSmmFacebookaccountComponent {
  responsedata: any;
  facebookpage_summarylist: any;
  facebook!: IFacebook;
  facebookpost!: IFacebookpost;
  platformpost!: IPlatform;
  FacebookImageForm!: FormGroup;
  Multipleimagepost!: FormGroup;
  Multipleplatformpost!: FormGroup;
  FacebookImagescheduleForm!: FormGroup;
  file!: File;
  parameterValue1: any;
  schedule_list: any;
  facebookpage_list: any;
  instagramaccount_list: any;
  showOptionsDivId: any;


  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.facebook = {} as IFacebook;
    this.facebookpost = {} as IFacebookpost;
    this.platformpost = {} as IPlatform

  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    const today = new Date();
    const minDate = today;
    const maxDate = new Date(today.getTime());

    const Options = {
      enableTime: true,
      dateFormat: 'Y-m-d H:i:S K',
      minDate: minDate,
      maxDate: maxDate,
      defaultDate: today,
      minuteIncrement: 1 // Set minute increment to 1 for 1-minute intervals
    };
    flatpickr('.date-picker', Options); // Initialize Flatpickr with options
    this.GetPagedetailssummary();
    this.FacebookImageForm = new FormGroup({
      image_caption: new FormControl(this.facebook.image_caption, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
        this.noWhitespaceValidator(),
      ]),
      file_path: new FormControl(this.facebook.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
    });
    this.Multipleimagepost = new FormGroup({
      image_caption: new FormControl(this.facebook.image_caption, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
        this.noWhitespaceValidator(),
      ]),
      file_path: new FormControl(this.facebook.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      page_name: new FormControl(this.facebookpost.page_name, [
        Validators.required,
      ]),

    });
    this.Multipleplatformpost = new FormGroup({
      image_caption: new FormControl(this.platformpost.image_caption, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
        this.noWhitespaceValidator(),
      ]),
      file_path: new FormControl(this.platformpost.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      page_name: new FormControl(this.platformpost.page_name, [
        Validators.required,
      ]),
      username: new FormControl(this.platformpost.username, [
        Validators.required,
      ]),
      mention: new FormControl(this.platformpost.mention, [
        Validators.pattern(/^(?!\s*$).+/),

      ]),
    });
    this.FacebookImagescheduleForm = new FormGroup({
      image_schedulercaption: new FormControl(this.facebook.image_schedulercaption, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
        this.noWhitespaceValidator(),
      ]),
      schedule_at: new FormControl(this.facebook.schedule_at, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      file_path: new FormControl(this.facebook.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
    });
  }
  toggleOptions(facebook_page_id: any) {
    if (this.showOptionsDivId === facebook_page_id) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = facebook_page_id;
    }
  }
  get image_caption() {
    return this.FacebookImageForm.get('image_caption')!;
  }
  get page_name() {
    return this.Multipleimagepost.get('page_name')!;

  }
  get caption() {
    return this.Multipleimagepost.get('image_caption')!;
  }
  get image_schedulercaption() {
    return this.FacebookImagescheduleForm.get('image_schedulercaption')!;
  }
  get schedule_at() {
    return this.FacebookImagescheduleForm.get('schedule_at')!;
  }
  get username() {
    return this.Multipleplatformpost.get('username')!;
  }

  GetPagedetailssummary() {
    this.NgxSpinnerService.show();
    var url = 'Facebook/GetPagedetailssummary'
    this.service.get(url).subscribe((result: any) => {
      // window.location.reload()
      this.responsedata = result;
      this.facebookpage_summarylist = this.responsedata.facebookpage_summarylist;
      setTimeout(() => {
        $('#facebookpage_summarylist').DataTable();
      }, 100);
      this.NgxSpinnerService.hide();

    });
  }
  postview(facebook_page_id: any): void {
    const secretKey = 'storyboarderp';
    facebook_page_id = AES.encrypt(facebook_page_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmFacebookPage', facebook_page_id]);
  }
  noWhitespaceValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isWhitespace = (control.value || '').trim().length === 0;
      return isWhitespace ? { whitespace: true } : null;
    };
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  onclose() {
    this.FacebookImageForm.reset();
    this.FacebookImagescheduleForm.reset();
    this.Multipleimagepost.reset();
  }
  postimage(facebook_page_id: any) {
    this.parameterValue1 = facebook_page_id
  }
  public onsubmit(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_caption", this.FacebookImageForm.value.image_caption);
      formData.append("page_id", this.parameterValue1);
      this.NgxSpinnerService.show();
      var api = 'Facebook/UploadImage'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.FacebookImageForm.reset();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.FacebookImageForm.reset();
        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image/Video !!")


    }
  }
  public onmultipleimagepost(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_caption", this.Multipleimagepost.value.image_caption);
      formData.append("page_name", this.Multipleimagepost.value.page_name);
      this.NgxSpinnerService.show();
      var api = 'Facebook/Multiplepagepost'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.Multipleimagepost.reset();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.Multipleimagepost.reset();
        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image/Video !!")


    }
  }
  public scheduleonsubmit(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined && this.schedule_at != null && this.schedule_at != undefined && this.image_schedulercaption != null && this.image_schedulercaption != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_schedulercaption", this.FacebookImagescheduleForm.value.image_schedulercaption);
      formData.append("schedule_at", this.FacebookImagescheduleForm.value.schedule_at);
      formData.append("file", this.parameterValue1);
      this.NgxSpinnerService.show();

      var api = 'Facebook/ScheduleUploadImage'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.FacebookImagescheduleForm.reset();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.FacebookImagescheduleForm.reset();
        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image/Video !!")
    }
  }
  summaryrefresh() {
    this.NgxSpinnerService.show();
    var url1 = 'Facebook/GetPagedetails'
    this.service.get(url1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;
      window.location.reload();

    });

  }
  logdetails(facebook_page_id: any): void {
    // this.NgxSpinnerService.show();
    var url2 = 'Facebook/Getschedulelogdetails'
    let params = {
      page_id: facebook_page_id
    }

    this.service.getparams(url2, params).subscribe((result: any) => {
      this.schedule_list = result.schedulelist;

    });
  }
  pagelist() {
    var url1 = 'Facebook/Getpagelist'
    this.service.get(url1).subscribe((result: any) => {
      this.facebookpage_list = result.facebookpage_list;
      this.Instaaccountlist();
    });

  }
  Instaaccountlist() {
    var url1 = 'Facebook/Getinstagramaccountlist'
    this.service.get(url1).subscribe((result: any) => {
      this.instagramaccount_list = result.instagramaccount_list;

    });
  }
  public onmultiplesocialpost(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_caption", this.Multipleplatformpost.value.image_caption);
      formData.append("page_name", this.Multipleplatformpost.value.page_name);
      this.NgxSpinnerService.show();
      var api = 'Facebook/Multiplepagepost'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.Multipleplatformpost.reset();
        }
        else {
          formData.append("instaaccount_id", this.Multipleplatformpost.value.username);
          formData.append("mention", this.Multipleplatformpost.value.mention);
          var api = 'Facebook/Postinstaplatformpost'
          this.service.postfile(api, formData).subscribe((result: any) => {

            if (result.status == false) {
              this.NgxSpinnerService.hide();
              this.ToastrService.warning(result.message)
              this.Multipleplatformpost.reset();
            }
            else {
              this.NgxSpinnerService.hide();
              this.ToastrService.success(result.message)
              this.Multipleplatformpost.reset();
            }
          });
        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image/Video !!")


    }
  }
}