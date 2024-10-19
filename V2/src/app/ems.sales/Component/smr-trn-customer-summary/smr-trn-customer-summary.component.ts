import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { ExcelService } from 'src/app/Service/excel.service';
interface ICustomer {
  customer_gid: string;
  customer_id: string;
  customer_name: string;
  contact_details: string;
  region_name: string;
  PopupControlExtender_branch: string;
  PopupControlExtender_person: string;
  customer_type: string;
}
@Component({
  selector: 'app-smr-trn-customer-summary',
  templateUrl: './smr-trn-customer-summary.component.html',
  styleUrls: ['./smr-trn-customer-summary.component.scss'],
  // styles: [`
  // table thead th, 
  // .table tbody td { 
  //  position: relative; 
  // z-index: 0;
  // } 
  // .table thead th:last-child, 
  
  // .table tbody td:last-child { 
  //  position: sticky; 
  
  // right: 0; 
  //  z-index: 0; 
  
  // } 
  // .table td:last-child, 
  
  // .table th:last-child { 
  
  // padding-right: 50px; 
  
  // } 
  // .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
  //  background-color: #ffffff; 
    
  //   } 
  //   .table.table-striped tbody tr:nth-child(even) td:last-child { 
  //    background-color: #f2fafd; 
  
  // } 
  // `]
})
export class SmrTrnCustomerSummaryComponent {
  file!: File;
  reactiveForm!: FormGroup;
  responsedata: any;
  customercount_list :any;
  filter_list: any[]=[];
  total_list: number=0;
  currentPage: number = 1;
  itemsPerPage: number = 200;
  sortBy: string = '';
  sortOrder: boolean = true; 
  itemsPerPageOptions: number[] = [200, 500,1000, 1500];
  noResultsMessage:any;
  parameter: any;
  eportalform!:FormGroup;
  parameterValue: any;
  parameterValue1: any;
  showOptionsDivId:any;
  customertype_list :any;
  firstCustomertype :any;
  firstCustomertype2:any;
  showhide: boolean=true;
  firstCustomertype3:any;
  password:string | any;
  confirmPasswordTouched = false;
  confirmpassword:string | any;
  showPassword: boolean = false;
  showConfrimPassword: boolean = false;
  smrcustomer_list: any[] = [];
  customertotalcount_list: any[] = [];
  Documentdtl_list: any[] = [];
  Document_list: any[] = [];
  customerordercount :any[] = [];
  customer!: ICustomer;
  getData: any;
  constructor(private formBuilder: FormBuilder, private excelService : ExcelService,
    private ToastrService: ToastrService, private router: ActivatedRoute, 
    private route: Router, public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService,) {
    this.customer = {} as ICustomer;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetSmrTrnCustomerSummary();
    // this.GetCustomerTypeSummary();

    this.eportalform = new FormGroup({
      customer_gid: new FormControl(''),
      eportalemail_id:new FormControl('',[Validators.required]),
      password:new FormControl('',
        [
          Validators.required,
          Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
        ]),
      confirmpassword:new FormControl('',
      [
        Validators.required,
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
      ])
    });
    
}
get eportalemail_id(){
  return this.eportalform.get('eportalemail_id')!;
}
// get password(){
//  return this.eportalform.get('password')!;
// }

// get confirmpassword() {
//   return this.eportalform.get('confirmpassword')!;
// }
get customer_gid(){
  return this.eportalform.get('customer_gid')!;
}
passwordsMatch(): boolean {
  const password = this.eportalform.get('password')?.value;
  const confirmPassword = this.eportalform.get("confirmpassword")?.value
  return password === confirmPassword;
}
togglePasswordVisibility(): void {
  this.showPassword = !this.showPassword;
}
toggleconfirmPasswordVisibility(){
  this.showConfrimPassword = !this.showConfrimPassword;
}
toggleOptions(account_gid: any) {
  if (this.showOptionsDivId === account_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = account_gid;
  }
}

GetCustomerTypeSummary() {
  var api = 'SmrTrnCustomerSummary/GetCustomerTypeSummary'
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.customertype_list = result.customertype_list1;    
    this.firstCustomertype = this.customertype_list[0].customer_type1;
    this.firstCustomertype2 = this.customertype_list[1].customer_type1;
    this.firstCustomertype3 = this.customertype_list[2].customer_type1;  
  });
}
//// Summary Grid//////
GetSmrTrnCustomerSummary() {
  var url = 'SmrTrnCustomerSummary/GetSmrTrnCustomerSummary'
  this.NgxSpinnerService.show();
  this.service.get(url).subscribe({
   next: (result: any) => {
 $('#smrcustomer_list').DataTable().destroy();
     this.smrcustomer_list = result.smrcustomer_list || [];;
     this.filter_list = this.smrcustomer_list;
     this.total_list = this.smrcustomer_list.length;

     this.NgxSpinnerService.hide();
     
     setTimeout(() => {
       $('#smrcustomer_list').DataTable()
     }, 1);
  }
}); 
  

// var url  = 'SmrTrnCustomerSummary/GetSmrTrnCustomerCount';
//     this.service.get(url).subscribe((result:any) => {
//     this.responsedata = result;
//     this.customercount_list = this.responsedata.customercount_list; 
//     console.log(this.customercount_list);
//     });

    var url  = 'SmrTrnCustomerSummary/GetSmrTrnCustomertotalCount';
    this.service.get(url).subscribe((result:any) => {
    this.responsedata = result;
    this.customertotalcount_list = this.responsedata.customertotalcount_list; 
    console.log(this.customercount_list);
    });

  }

  get page_list(): any[] {
    const startIndex =(this.currentPage -1 ) * this.itemsPerPage
    const endIndex = startIndex + this.itemsPerPage;
    return this.filter_list.slice(startIndex, endIndex);
  }
  sort(field: string) {
    if (this.sortBy === field) {
      this.sortOrder = !this.sortOrder; // Toggle sort order
    } else {
      this.sortBy = field;
      this.sortOrder = true; // Default to ascending
    }
    this.filter_list.sort((a: any, b: any) => {
      if (a[field] < b[field]) {
        return this.sortOrder ? -1 : 1;
      } else if (a[field] > b[field]) {
        return this.sortOrder ? 1 : -1;
      } else {
        return 0;
      }
    });
  }
  get startIndex(): number {
    return (this.currentPage - 1) * this.itemsPerPage;
  }
  
  get endIndex(): number {
    return Math.min(this.startIndex + this.itemsPerPage, this.total_list);
  }
  onItemsPerPageChange(): void {
    this.currentPage = 1;
  }
  search(event: Event) {
    const input = event.target as HTMLInputElement;
    const query = input?.value.trim().toLowerCase(); // Trim whitespace and convert to lowercase
    if (!query) {
      this.filter_list = this.smrcustomer_list;
    } else {
      this.filter_list = this.smrcustomer_list.filter((item: any) => {
        // Convert all properties to lowercase before comparing
        const customer_id = item.customer_id.toLowerCase();
        const customer_name = item.customer_name.toLowerCase();
        const region_name = item.region_name.toLowerCase();
        const contact_details = item.contact_details.toLowerCase();
  
        // Filter items based on the main list properties
        const matchesMainList = 
        customer_id.includes(query) ||
        customer_name.includes(query) ||
        region_name.includes(query) ||
        contact_details.includes(query);
  

        return matchesMainList;
      });
    }
    this.total_list = this.filter_list.length; 
    this.currentPage = 1; 
    if (this.total_list === 0) {
      
      this.noResultsMessage = "No matching records found";
    } else {
      this.noResultsMessage = "";
    }
  }
  pageChanged(event: any): void {
    this.currentPage = event.page;
  }

  toggleStatus(data: any) {

    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }
onview(params:any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage = "/smr/SmrTrnCustomerSummary";
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SrmTrnCustomerview',encryptedParam,lspage]) 
}
onedit(params: any){
  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage = "/smr/SmrTrnCustomerSummary";
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrMstCustomerEdit',encryptedParam,lspage]) 
}
onaddinfo(){}


openModalinactive(parameter: string){
  this.parameterValue = parameter
}


oninactive(){
  console.log(this.parameterValue);
    var url3 = 'SmrTrnCustomerSummary/GetcustomerInactive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if ( result.status == false) {
       this.ToastrService.warning('Error While Customer Inactivated')
      }
      else {
       this.ToastrService.success('Customer Inactivated Successfully')
        }
        this.GetSmrTrnCustomerSummary();
    });
   
}

openModalactive(parameter: string){
  this.parameterValue = parameter
}
onclose() {
  this.reactiveForm.reset();

}

onactive(){
  console.log(this.parameterValue);
    var url3 = 'SmrTrnCustomerSummary/GetcustomerActive'
    this.service.getid(url3, this.parameterValue).subscribe((result: any) => {

      if ( result.status == false) {
       this.ToastrService.warning('Error While Customer Activated')
      }
      else {
       this.ToastrService.success('Customer Activated Successfully')
        }
        this.GetSmrTrnCustomerSummary();
    });
   
}


onprod(params: any)
{


  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage = "/smr/SmrTrnCustomerSummary";
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerPriceSegment',encryptedParam, lspage]) 
}

importexcel() {
  let formData = new FormData();
  if (this.file != null && this.file != undefined) {
    window.scrollTo({
      top: 0, // Code is used for scroll top after event done
    });
    formData.append("file", this.file, this.file.name);
    var api = 'SmrTrnCustomerSummary/CustomerImport'
    this.NgxSpinnerService.show();
    this.service.postfile(api, formData).subscribe((result: any) => {
      this.responsedata = result;       
       if(result.status ==false){
        this.ToastrService.warning("Kindly Fill All Mandatory Fields!")     
        this.NgxSpinnerService.hide(); 
        this.GetSmrTrnCustomerSummary();           
      }
      else{
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide(); 
        this.GetSmrTrnCustomerSummary();
      }
      
    });
  }
}
onChange1(event: any) {
  this.file = event.target.files[0];
}
downloadfileformat() {
  let link = document.createElement("a");
  link.download = "Customer Details";
  window.location.href = "https://"+ environment.host + "/Templates/Customer Details.xlsx";
  link.click();
}
onbranch(params: any)
{

  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage = "/smr/SmrTrnCustomerSummary";
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerbranch',encryptedParam, lspage]) 
}
oncontact(params: any)
{

  const secretKey = 'storyboarderp';
  const param = (params);
  const lspage = "/smr/SmrTrnCustomerSummary";
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerCall',encryptedParam, lspage]) 
}
// getdocumentlist(){

//   var api1='SmrTrnCustomerSummary/GetDocumentlist'
//      this.service.get(api1).subscribe((result:any)=>{
//     this.responsedata=result;
//     this.Document_list = this.responsedata.document_list1;
//       });    
// }

ondetail(document_name:any){

  var api1='SmrTrnCustomerSummary/GetDocumentDtllist'
  var param={
    document_gid:document_name,
  }
  this.service.getparams(api1,param).subscribe((result:any)=>{
 
    this.responsedata=result;
    this.Documentdtl_list = this.responsedata.documentdtl_list;  
  });
}


customerexportExcel() {
  // const CustomerExcel = this.smrcustomer_list.map(item => ({
  //   CustomerCode: item.customer_id || '', 
  //   Customer : item.customer_name || '',
  //   CustomerType : item.customer_type || '',
  //   ContactDetails : item.contact_details || '',
  //   Region : item.region_name || '',
  //   CustomerSince : item.customer_since || '',
  //   LastOrderRaisedOn : item.last_order_date || '',
    
    
  // }));

        
  //       this.excelService.exportAsExcelFile(CustomerExcel, 'Customer_Excel');
  

  var api7 =  'SmrTrnCustomerSummary/GetCustomerReportExport';
    this.service.generateexcel(api7).subscribe((result: any) => {
        this.responsedata = result;

        if (this.responsedata && this.responsedata.customerexport_list && this.responsedata.customerexport_list.length > 0) {
            var phyPath = this.responsedata.customerexport_list[0].lspath1;
            var relPath = phyPath.split("EMS_Base");
            var hosts = environment.host;
            var prefix = location.protocol + "//";
            var str = prefix.concat(hosts, relPath[1]);

            var link = document.createElement("a");
            var name = this.responsedata.customerexport_list[0].lsname2;
            link.download = name; 
            link.href = str;
            link.click();
        } else {
            console.error('No export data available');
        }
    }, error => {
        console.error('Error generating Excel file', error);
    });

}


lastorders(customer_gid:any){

  var api1='SmrTrnCustomerSummary/GetLastFiveOrderSummary'
  var param={
    customer_gid:customer_gid,
  }
  this.service.getparams(api1,param).subscribe((result:any)=>{
 
    this.responsedata=result;
    this.customerordercount = this.responsedata.fiveorder_list;  
  });
}

retailer(customertype_gid:any)
{
  const secretKey = 'storyboarderp';
  const param = (customertype_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerRetailer',encryptedParam]) 
}

distributor(customertype_gid:any)
{
  const secretKey = 'storyboarderp';
  const param = (customertype_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerDistributor',encryptedParam]) 
}

corporate(customertype_gid:any)
{
  const secretKey = 'storyboarderp';
  const param = (customertype_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnCustomerCorporate',encryptedParam]) 
}

// Customer 360

Onopen(leadbank_gid:any,lead2campaign_gid:any,leadbankcontact_gid:any)
  {

  const secretKey = 'storyboard';
  const lspage1 = "/smr/SmrTrnCustomerSummary";
  const lspage = AES.encrypt(lspage1, secretKey).toString();
  const param = (leadbank_gid);
  const param1 = (lead2campaign_gid);
  const param2 = (leadbankcontact_gid);
  const encryptedParam = AES.encrypt(param,secretKey).toString();
  const encryptedParam1 = AES.encrypt(param1,secretKey).toString();
  const encryptedParam2 = AES.encrypt(param2,secretKey).toString();
  this.route.navigate(['/smr/SmrTrnSales360',encryptedParam,encryptedParam1,encryptedParam2,lspage]) 
  }
  oneportal(customer_gid: string){
    this.eportalform.reset()
    this.parameterValue1 = customer_gid
     this.eportalform.get("customer_gid")?.setValue(customer_gid);
  }
  

  oneportalclose(){
    this.eportalform.reset()
  }
  
  oneportalsubmit()
  {
    debugger
    const secretKey = 'storyboarderp';
    const param = this.eportalform.value.customer_gid
    const param1 = this.eportalform.value.eportalemail_id
    const param2 = this.eportalform.value.password
    const encryptedParam =AES.encrypt(param,secretKey).toString();
    const encryptedParam1 =AES.encrypt(param1,secretKey).toString();
    const encryptedParam2 =AES.encrypt(param2,secretKey).toString();
    this.route.navigate(['/smr/customereportalpreviewmail',encryptedParam,encryptedParam1,encryptedParam2])
    // var url = "SmrTrnCustomerSummary/eportalmail";
    // this.NgxSpinnerService.show()
    // this.service.postparams(url,this.eportalform.value).subscribe((result:any)=>{
    //   if(result.status == true){
    //     this.ToastrService.success(result.message)
    //     this.NgxSpinnerService.hide()
    //   }
    //   else{
    //     this.ToastrService.warning(result.message)
    //     this.NgxSpinnerService.hide()
    //   }
    // })
  }
}