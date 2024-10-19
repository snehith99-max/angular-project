import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, PatternValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { saveAs } from 'file-saver';
import { NgxSpinnerService } from 'ngx-spinner';

interface IProposal {
  proposal_gid: string;
  template_gid: string;
  proposal_name: string;
  raiseproposal_list: string;
  enquiry_gid: string;
  customer_name: any;
   file:string;
  template_name: string;
  propsoal_document :string;
}

@Component({
  selector: 'app-smr-trn-raiseproposal',
  templateUrl: './smr-trn-raiseproposal.component.html',
  styleUrls: ['./smr-trn-raiseproposal.component.scss']
})
export class SmrTrnRaiseproposalComponent {
  file!: File;
  data: any;
  proposal!: IProposal;
  enquiry_gid: any;
  reactiveForm!: FormGroup;
  document_list: any[] = [];
  responsedata: any;
  raiseproposal_list: any;
  GetDocumentType: any;
  postproposal_list :any;
  proposal_list: any;
  sample: any;
  template :any;
  proposalsummary_list : any;
  formDataObject: FormData = new FormData();
  allattchement: any[] = [];
  parameterValue: any;

  constructor(private renderer: Renderer2, public NgxSpinnerService:NgxSpinnerService,private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private router: Router, private route: ActivatedRoute) {
    this.proposal = {} as IProposal;
  }
  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      proposal_gid: new FormControl(''),
      enquiry_gid: new FormControl(''),
      proposal_name: new FormControl(this.proposal.proposal_name, [
        Validators.required,
      ]),
      file: new FormControl(''),
      Model : new FormControl(''),
      customer_name: new FormControl(null),
      proposal_document: new FormControl(''),
      template_name : new FormControl(''),
      customer_gid: new FormControl(''),
      leadbank_gid: new FormControl(''),
      quotation_gidL: new FormControl('')
      
    });

    this.enquiry_gid = this.route.snapshot.paramMap.get('enquiry_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.enquiry_gid, secretKey).toString(enc.Utf8);
    this.enquiry_gid = deencryptedParam
this.GetSmrTrnRaiseProposal(this.enquiry_gid)
   
    
  }
  downloadImage(data: any) {
    if (data.document_path != null && data.document_path != "") {
      if (data.proposal_from === 'enquiry') {
        saveAs(data.document_path, `${data.proposal_gid}_file`);
      }
   
      else {
        saveAs(data.document_path, `${data.proposal_gid}_file`);
      }
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Files Found') 
    }
 
  }
  
  GetSmrTrnRaiseProposal(enquiry_gid: any) {
    debugger
    var url1 = 'SmrTrnCustomerEnquiry/GetSmrTrnRaiseProposal'
    this.NgxSpinnerService.show()
    let param = {
      enquiry_gid: this.enquiry_gid
    }
    this.service.getparams(url1, param).subscribe((result: any) => {
      this.proposal_list = result.proposal_list;
      this.reactiveForm.get("customer_name")?.setValue(this.proposal_list[0].customer_name);
      this.reactiveForm.get("enquiry_gid")?.setValue(this.proposal_list[0].enquiry_gid);
      this.reactiveForm.get("customer_gid")?.setValue(this.proposal_list[0].customer_gid);
      this.reactiveForm.get("document_path")?.setValue(this.proposal_list[0].document_path);
      this.responsedata=result.proposalsummary_list;
      this.GetProposalSummary();
      this.NgxSpinnerService.hide()

    });
  
  }
  GetProposalSummary() {
    debugger
    var params = {
      enquiry_gid: this.enquiry_gid
    }
    var url = 'SmrTrnCustomerEnquiry/GetProposalSummary'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.proposalsummary_list = this.responsedata.proposalsummary_list;
    });
 }
   
  get proposal_name() {
    return this.reactiveForm.get('proposal_name')!;
  }
  get proposal_document() {
    return this.reactiveForm.get('proposal_document')!;
  }

  onupload() {
    console.log(this.reactiveForm.value)
    this.proposal = this.reactiveForm.value;  

      if (true) {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        formData.append("file", this.file, this.file.name);
       
        
        var api = 'SmrTrnCustomerEnquiry/Postpropsal'
        this.NgxSpinnerService.show()
        this.service.postfile(api, formData).subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
          }
        });
      }
      this.reactiveForm.reset();
      
    }  
    return;
  } 
  
  validate() {
    debugger
    this.proposal = this.reactiveForm.value;
    if (this.proposal.proposal_name != null) {
      let formData = new FormData();
      if (this.file != null && this.file != undefined) {
        debugger
        formData.append("proposal_name", this.proposal.proposal_name);
        

        var api = 'SmrTrnCustomerEnquiry/Postpropsal'
        this.NgxSpinnerService.show()
        this.service.postfile(api, formData).subscribe((result: any) => {
          debugger
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
            this.router.navigate(['/smr/SmrTrnRaiseproposal']);
          }
        });

      }
      else {
        var api7 = 'SmrTrnCustomerEnquiry/Postpropsal'
        this.NgxSpinnerService.show()
        this.service.post(api7, this.proposal).subscribe((result: any) => {

          if (result.status == false) {
            this.ToastrService.warning(result.message)
            this.NgxSpinnerService.hide()
          }
          else {
            this.ToastrService.success(result.message)
            this.NgxSpinnerService.hide()
            this.router.navigate(['/smr/SmrTrnRaiseproposal']);
          }
          
        });
      }
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  onSubmit() {

   console.log(this.reactiveForm)
   let formData = new FormData();
   if (this.file != null && this.file != undefined) {
   formData.append("file", this.file, this.file.name);
   formData.append("proposal_name",this.reactiveForm.value.proposal_name);
   formData.append("enquiry_gid",this.reactiveForm.value.enquiry_gid);
    }
    var api = 'SmrTrnCustomerEnquiry/Postpropsal';
    this.NgxSpinnerService.show()
    this.service.postfile(api, formData).subscribe((result: any) => {
      debugger
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.warning(result.message) 
        this.NgxSpinnerService.hide()
      }
      this.reactiveForm.get("proposal_name")?.setValue(null)
      this.reactiveForm.get("proposal_document")?.setValue(null)
      window.location.reload();
    });
  
  }

  onChange2(event:any) {
    this.file =event.target.files[0];

}

openModaldelete(parameter:string)
    {
      this.parameterValue = parameter
    }

    ondelete()
    {
      var url = 'SmrTrnCustomerEnquiry/DeleteProposal'
      this.NgxSpinnerService.show()
      let param = {
        proposal_gid: this.parameterValue
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide()
        }
        else {
  
          this.ToastrService.success(result.message)
          this.NgxSpinnerService.hide()
  
        }
        this.GetProposalSummary();
      });
    }
}