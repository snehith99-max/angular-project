import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute} from '@angular/router';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-crm-trn-contact-individual-view',
  templateUrl: './crm-trn-contact-individual-view.component.html',
  styleUrls: ['./crm-trn-contact-individual-view.component.scss']
})
export class CrmTrnContactIndividualViewComponent {
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];
  filesWithIds: { file: File, AutoIDkey: string }[] | any = [];
  AutoIDkey:any;
  contact_gid: any;
  Individual_list: any;
  contact_name: any;
  contact_ref_no: any;
  contact_type: any;
  pan_no: any;
  aadhar_no: any;
  DocumentList: any;
  mobile_list: any;
  email_list: any;
  mothercontact_no: any;
  promoter_list: any;
  director_list: any;
  AddressList: any;
  lgltrade_name: any;
  corporate_pan_no: any;
  lei: any;
  cin: any;
  cin_date: any;
  constitution: any;
  businessstart_date: any;
  businesss_vintage: any;
  tan: any;
  tan_state: any;
  kin: any;
  udhayam_registration: any;
  category_aml: any;
  category_business: any;
  last_year_turnover: any;
  gst_list: any;
  age: any;
  individual_dob: any;
  gender_name: any;
  maritalstatus_name:any;
  physicalstatus_name:any;
  designation_name:any;
  address1:any;
  address2:any;
  city:any;
  state:any;
  postal_code:any;
  country_name:any;
  latitude:any;
  longitude:any;
  tempaddress1 :any;
  tempaddress2 :any;
  tempcity :any;
  temppostal_code :any;
  tempcountry_name :any;
  templatitude :any;
  templongitude :any;
  father_name :any;
  fathercontact_no :any;
  mother_name :any;
  spouse_name :any;
  spousecontact_no :any;
  educationalqualification_name :any;
  main_occupation :any;
  annual_income :any;
  monthly_income :any;
  incometype_name :any;
  tempstate: any;
  regionname: any;
  sourcename: any;
  referred_by: any;

  constructor(private NgxSpinnerService: NgxSpinnerService,private SocketService: SocketService,private Location:Location,private ActivatedRoute: ActivatedRoute,private ToastrService:ToastrService){
  }
  ngOnInit(): void {
    this.NgxSpinnerService.show();
    const contact_gid = this.ActivatedRoute.snapshot.paramMap.get('leadbank_gid');
    this.contact_gid = contact_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.contact_gid, secretKey).toString(enc.Utf8);
    this.contact_gid = deencryptedParam;
      var param = {
        leadbank_gid: this.contact_gid,
      }
      this.NgxSpinnerService.show();
      var url= 'ContactManagement/ContactEditView';
      this.SocketService.getparams(url,param).subscribe((result:any)=>{
        this.contact_name = result.contact_name;
            this.contact_ref_no = result.contact_ref_no;
            this.contact_type = result.contact_type;
            this.pan_no = result.pan_no;
            this.aadhar_no = result.aadhar_no;
            this.individual_dob = result.individual_dob;
            this.age = result.age;
            this.gender_name = result.gender_name;
            this.designation_name = result.designation_name;
            this.maritalstatus_name = result.maritalstatus_name;
            this.physicalstatus_name = result.physicalstatus_name;
            this.address1 = result.address1;
            this.address2 = result.address2;
            this.city = result.city;
            this.state = result.state;
            this.postal_code = result.postal_code;
            this.country_name = result.country_name;
            this.latitude = result.latitude;
            this.longitude = result.longitude;
            this.tempaddress1 = result.tempaddress1;
            this.tempaddress2 = result.tempaddress2;
            this.tempcity = result.tempcity;
            this.tempstate = result.tempstate;
            this.temppostal_code = result.temppostal_code;
            this.tempcountry_name = result.tempcountry_name;
            this.templatitude = result.templatitude;
            this.templongitude = result.templongitude;
            this.father_name = result.father_name;
            this.fathercontact_no = result.fathercontact_no;
            this.mother_name = result.mother_name;
            this.mothercontact_no = result.mothercontact_no;
            this.spouse_name = result.spouse_name;
            this.spousecontact_no = result.spousecontact_no;
            this.educationalqualification_name = result.educationalqualification_name;
            this.main_occupation = result.main_occupation;
            this.annual_income = result.annual_income;
            this.monthly_income = result.monthly_income;
            this.incometype_name = result.incometype_name;
    
            this.mobile_list = result.mobile_list;
            this.email_list = result.email_list;
            this.DocumentList = result.DocumentList;
            this.sourcename = result.source_name;
            this.regionname = result.region_name;
            this.referred_by = result.referred_by;
            this.NgxSpinnerService.hide();
      });

  }
      
  // mobile_list=[
  //   {mobileno:'9994847855',primarystatus:'yes'},
  //   {mobileno:'9994843434',primarystatus:'no'},
  // ]
  
  // email_list=[
  //   {emaillist:'subash@vcidex.com',primarystatus:'yes'},
  //   {emaillist:'yesu@vcidex.com',primarystatus:'no'},
  // ]
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

  viewFile(path: string, name: string) {
    const lowerCaseFileName = name.toLowerCase();
      if (!(lowerCaseFileName.endsWith('.pdf') ||
            lowerCaseFileName.endsWith('.jpg') ||
            lowerCaseFileName.endsWith('.jpeg') ||
            lowerCaseFileName.endsWith('.png') ||
            lowerCaseFileName.endsWith('.txt'))){
              window.scrollTo({
                top: 0,
              });
          this.ToastrService.warning('Unsupported file format');
      }
      else {
        var params = {
          file_path: path,
          file_name: name
        }
        var url = 'TskTrnTaskManagement/DownloadDocument';
        this.SocketService.post(url, params).subscribe((result: any) => {
              if (result != null) {
                this.SocketService.fileviewer(result);
              }
            });
          }
    }

  downloadFiles(path: string, name: string) {
    var params = {
      file_path: path,
      file_name: name
    }
    var url = 'TskTrnTaskManagement/DownloadDocument';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if(result != null){
        this.SocketService.filedownload1(result);
      }
    });
  }
  backbutton(){
    this.Location.back() ;
  }
}
