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
import { AES } from 'crypto-js';
interface IInstagram {
  image_caption: string;
  image_caption1: string;
  file_path: string;
  video_caption: string;
  image_schedulercaption: string;
  schedule_at: string;
  mention: string;

}
interface Carousel {
  image_caption: string;
  mention:string;
}

interface IFacebookpost {
  caption: string;
  file_path: string;
  image_schedulercaption: string;
  schedule_at: string;
  page_name: string
}

@Component({
  selector: 'app-crm-smm-instagram',
  templateUrl: './crm-smm-instagram.component.html',
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
export class CrmSmmInstagramComponent {
  responsedata: any;
  instagramaccount_summarylist: any;
  instagram!: IInstagram;
  carousel!: Carousel;

  facebookpost!: IFacebookpost;

  InstaImageForm!: FormGroup;
  InstaReelForm!: FormGroup;
  InstastoriesForm!: FormGroup;
  carouselpost!: FormGroup;
  FacebookImagescheduleForm!: FormGroup;
  file!: File;
  parameterValue1: any;
  schedule_list: any;
  facebookpage_list: any;
  ////multiple upload start////

  formDataObject: FormData = new FormData();
  file_name: any;
  AutoIDkey: any;
  DocumentForm: FormGroup | any;
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];
  documentData_list: any[] = [];
  Documentname: any;
  showOptionsDivId: any;
  formData: FormData = new FormData();

  ////multiple upload end////


  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, private route: Router, public service: SocketService, private router: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService) {
    this.instagram = {} as IInstagram;
    this.carousel = {} as Carousel;

    this.facebookpost = {} as IFacebookpost;
    this.camdoucument()


  }
  camdoucument() {
    this.DocumentForm = this.formBuilder.group({
      documentcam: ['', Validators.required]

    });
  }

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.summaryrefresh();
    this.Getaccountdetailsummary();
    this.InstaImageForm = new FormGroup({
      image_caption: new FormControl(this.instagram.image_caption, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      file_path: new FormControl(this.instagram.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      mention: new FormControl(this.instagram.mention, [
        Validators.pattern(/^(?!\s*$).+/),
      ]),
    });
    this.InstastoriesForm = new FormGroup({
      file_path: new FormControl(this.instagram.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
    });
    this.InstaReelForm = new FormGroup({
      image_caption1: new FormControl(this.instagram.image_caption1, [
        Validators.required,
        Validators.pattern(/^(?!\s*$).+/),
      ]),
      file_path: new FormControl(this.instagram.file_path, [
        Validators.required,
        this.noWhitespaceValidator(),
      ]),
      mention: new FormControl(this.instagram.mention, [
        Validators.pattern(/^(?!\s*$).+/),
      ]),
    });
    this.carouselpost = new FormGroup({
      image_caption: new FormControl(this.carousel.image_caption, [
        Validators.required,
        this.noWhitespaceValidator(),
      ])
    });

  }
  get image_caption() {
    return this.InstaImageForm.get('image_caption')!;
  }

  get image_caption1() {
    return this.InstaReelForm.get('image_caption1')!;
  }
  toggleOptions(callresponse_gid: any) {
    if (this.showOptionsDivId === callresponse_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = callresponse_gid;
    }
  }
  Getaccountdetailsummary() {
    this.NgxSpinnerService.show();
    var url = 'Instagram/Getaccountdetailsummary'
    this.service.get(url).subscribe((result: any) => {
      // window.location.reload()
      this.responsedata = result;
      this.instagramaccount_summarylist = this.responsedata.instagramaccount_summarylist;
      setTimeout(() => {
        $('#instagramaccount_summarylist').DataTable();
      }, 100);
      this.NgxSpinnerService.hide();
    });
  }
  postview(account_id: any): void {
    const secretKey = 'storyboarderp';
    account_id = AES.encrypt(account_id, secretKey).toString();
    this.route.navigate(['/crm/CrmSmmInstagrampage', account_id]);
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
    this.InstaImageForm.reset();
    this.InstaReelForm.reset();
    this.carouselpost.reset();
    this.DocumentForm.reset();
    this.documentData_list = [];
    this.InstastoriesForm.reset();

  }
  postimage(account_id: any) {
    this.parameterValue1 = account_id
  }
  public onimagesubmit(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_caption", this.InstaImageForm.value.image_caption);
      formData.append("account_id", this.parameterValue1);
      formData.append("mention", this.InstaImageForm.value.mention);

      this.NgxSpinnerService.show();
      var api = 'Instagram/PostInstaimage'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.InstaImageForm.reset();
          
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.InstaImageForm.reset();
        }
        this.summaryrefresh();
        this.Getaccountdetailsummary();
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image !!")


    }
  }
  public onreelsubmit(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("image_caption1", this.InstaReelForm.value.image_caption1);
      formData.append("account_id", this.parameterValue1);
      formData.append("mention", this.InstaReelForm.value.mention);

      this.NgxSpinnerService.show();
      var api = 'Instagram/PostInstareel'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.InstaReelForm.reset();

        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.InstaReelForm.reset();
        }
        this.summaryrefresh();
        this.Getaccountdetailsummary();
      });

    } else {
      this.ToastrService.warning("Kindly Upload Video !!")


    }
  }
  summaryrefresh() {
    this.NgxSpinnerService.show();
    var url1 = 'Instagram/Getaccountdetails'
    this.service.get(url1).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      this.responsedata = result;

    });
  }
  ////// multiple  upload start///
  DocumentClick() {
    const fileInput: HTMLInputElement = document.getElementById('fileInput') as HTMLInputElement;

    if (fileInput) {
      const files: FileList | null = fileInput.files;

      if (files != null && files.length != 0) {
        for (let i = 0; i < files.length; i++) {
          const file = files[i];
          this.AutoIDkey = this.generateKey() + this.documentData_list.length + 1;
          this.formDataObject.append(this.AutoIDkey, file);
          this.file_name = file.name;
          this.documentData_list.push({
            AutoID_Key: this.AutoIDkey,
            file_name: file.name
          });
          this.filesWithId.push({
            file,
            AutoIDkey: this.AutoIDkey,
            file_name: file.name
          });
        }
        fileInput.value = '';
        this.DocumentForm.reset();
      } else {
        this.ToastrService.warning("Kindly Upload the Document");
      }
    }
  }
  generateKey(): string {

    return `AutoIDKey${new Date().getTime()}`;
  }
  viewFile(AutoIDkey: string): void {
    const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);

    if (fileObject) {
      const file = fileObject.file;
      const contentType = this.getFileContentType(file);

      if (contentType) {
        const blob = new Blob([file], { type: contentType });
        const fileUrl = URL.createObjectURL(blob);
        const newTab = window.open(fileUrl, '_blank');

        if (newTab) {
          newTab.focus();
          setTimeout(() => {
            URL.revokeObjectURL(fileUrl);
          }, 60000); // Revokes the object URL after 60 seconds
        } else {
          console.error('Failed to open new tab.');
        }
      } else {
        window.scrollTo({ top: 0 });
        this.ToastrService.warning('Unsupported file format');
      }
    } else {
      console.error('File not found for AutoIDkey:', AutoIDkey);
    }
  }

  getFileContentType(file: File): string | null {
    const lowerCaseFileName = file.name.toLowerCase();

    if (lowerCaseFileName.endsWith('.pdf')) {
      return 'application/pdf';
    } else if (lowerCaseFileName.endsWith('.jpg') || lowerCaseFileName.endsWith('.jpeg')) {
      return 'image/jpeg';
    } else if (lowerCaseFileName.endsWith('.png')) {
      return 'image/png';
    } else if (lowerCaseFileName.endsWith('.txt')) {
      return 'text/plain';
    }
    return null;
  }
  DeleteDocumentClick(index: any) {
    if (index >= 0 && index < this.documentData_list.length) {
      this.documentData_list.splice(index, 1);
    }

  }
  public oncarouselsubmit(): void {
    {
      // formData.append('documentData_list', jsonData);
      const jsonData = "" + JSON.stringify(this.documentData_list) + "";
      this.formDataObject.append('documentData_list', jsonData);
      this.formDataObject.append("image_caption", this.carouselpost.value.image_caption);
      this.formDataObject.append("account_id", this.parameterValue1);

      this.NgxSpinnerService.show();
      var api = 'Instagram/carouselpost'
      this.service.postfile(api, this.formDataObject).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message);
          this.carouselpost.reset();
          this.documentData_list = [];
          this.DocumentForm.reset();
        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.carouselpost.reset();
          this.documentData_list = [];
          this.DocumentForm.reset();
        }
        this.summaryrefresh();
        this.Getaccountdetailsummary();
        window.location.reload();

      });


    }
  }
  ////// multiple upload ends//
  public onstorysubmit(): void {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("account_id", this.parameterValue1);
      this.NgxSpinnerService.show();
      var api = 'Instagram/PostInstastory'
      this.service.postfile(api, formData).subscribe((result: any) => {

        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
          this.InstastoriesForm.reset();

        }
        else {
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)
          this.InstastoriesForm.reset();
        }
      });

    } else {
      this.ToastrService.warning("Kindly Upload Image !!")


    }
  }
}