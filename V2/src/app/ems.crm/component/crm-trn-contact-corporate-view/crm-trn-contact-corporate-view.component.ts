import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute} from '@angular/router';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-crm-trn-contact-corporate-view',
  templateUrl: './crm-trn-contact-corporate-view.component.html',
  styleUrls: ['./crm-trn-contact-corporate-view.component.scss']
})
export class CrmTrnContactCorporateViewComponent {
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
  individual_dob: any;
  age: any;
  gender_name: any;
  designation_name: any;
  maritalstatus_name: any;
  physicalstatus_name: any;
  address1: any;
  address2: any;
  city: any;
  state: any;
  postal_code: any;
  country_name: any;
  latitude: any;
  longitude: any;
  tempaddress1: any;
  tempaddress2: any;
  tempcity: any;
  tempstate: any;
  temppostal_code: any;
  tempcountry_name: any;
  templatitude: any;
  templongitude: any;
  father_name: any;
  fathercontact_no: any;
  sourcename: any;
  referred_by: any;
  regionname: any;
  mother_name: any;
  spouse_name: any;
  spousecontact_no: any;
  educationalqualification_name: any;
  main_occupation: any;
  annual_income: any;
  monthly_income: any;
  incometype_name: any;
  DocumentList: any;
  mobile_list: any;
  email_list: any;
  mothercontact_no: any;
  lgltrade_name: any;
  corporate_pan_no: any;
  lei: any;
  constitution: any;
  tan: any;
  tan_state: any;
  cin: any;
  cin_date: any;
  businessstart_date: any;
  businesss_vintage: any;
  kin: any;
  udhayam_registration: any;
  category_aml: any;
  category_business: any;
  gst_list: any;
  AddressList: any;
  promoter_list: any;
  director_list: any;
  last_year_turnover: any;

  
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
         this.lgltrade_name = result.lgltrade_name;
        this.contact_ref_no = result.contact_ref_no;
        this.contact_type = result.contact_type;
        this.corporate_pan_no = result.corporate_pan_no;
        this.lei = result.lei;
        this.cin = result.cin;
        this.cin_date = result.cin_date;
        this.constitution = result.constitution;
        this.businessstart_date = result.businessstart_date;
        this.businesss_vintage = result.businesss_vintage;
        this.tan = result.tan;
        this.tan_state = result.tan_state;
        this.kin = result.kin;
        this.udhayam_registration = result.udhayam_registration;
        this.category_aml = result.category_aml;
        this.category_business = result.category_business;
        this.last_year_turnover = result.last_year_turnover;


        this.gst_list = result.gst_list;
        this.promoter_list = result.promoter_list;
        this.director_list = result.director_list;
        this.AddressList = result.address_list;
        this.DocumentList = result.DocumentList;
        this.sourcename = result.source_name;
        this.regionname = result.region_name;
        this.referred_by = result.referred_by;
        this.NgxSpinnerService.hide();
      });
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

  downloadFiles(path: string, name: string) {debugger
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
    // this.SocketService.downloadFile(params).subscribe((data: any) => {
    //   if (data != null) {
    //     this.SocketService.filedownload(data);
    //   }
    // });
  }
  backbutton(){
    this.Location.back() 
  }
}
