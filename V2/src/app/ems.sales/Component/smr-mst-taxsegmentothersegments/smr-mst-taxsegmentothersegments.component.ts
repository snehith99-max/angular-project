import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SelectionModel } from '@angular/cdk/collections';
import { Table } from 'primeng/table';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';



export class IMapTaxsegment{
  GetotherSegment_list  : string[] =[];
  customer_gid: any ;
  taxsegment_gid: any;
  customer_gid1: any[]=[];
}

@Component({
  selector: 'app-smr-mst-taxsegmentothersegments',
  templateUrl: './smr-mst-taxsegmentothersegments.component.html',
  styleUrls: ['./smr-mst-taxsegmentothersegments.component.scss']
})
export class SmrMstTaxsegmentothersegmentsComponent {

  GetotherSegment_list: any[]=[];
  GetTaxSegmentDropDown_list: any[]=[];
  taxsegment_gid: any;
  pick: Array<any> = [];
  selection1 = new SelectionModel<IMapTaxsegment>(true, []);
  CurObj1: IMapTaxsegment = new IMapTaxsegment();
  customer_gid1:any;
  mdltaxsegment:any;
  TaxSegmentCustomer_list:any[]=[];
  responsedata:any;

  taxsegmentform!: FormGroup;
constructor (public service: SocketService,
  private NgxSpinnerService: NgxSpinnerService,
	public ToastrService: ToastrService){}

ngOnInit() : void {
  this.taxsegmentform =new FormGroup({
    taxsegment_gid: new FormControl(''),
})
  var othersegmentapi = 'SmrMstTaxSegment/GetOtherSegamentSummary';
  $('#GetotherSegment_list').DataTable().destroy();
  this.service.get(othersegmentapi).subscribe((apiresponse: any) =>{
  this.GetotherSegment_list = apiresponse.GetotherSegment_list;
  this.taxsegment_gid = apiresponse.GetotherSegment_list[0].taxsegment_gid;
  this.GetTaxSegment(this.taxsegment_gid);
  
  setTimeout(() => {
    $('#GetotherSegment_list').DataTable();
  }, 1);
  });  
  this.totalcount()
  
}
totalcount(){
  var api = 'SmrMstTaxSegment/GetCustomerCount'
  this.service.get(api).subscribe((result: any) => {
    this.responsedata = result;
    this.TaxSegmentCustomer_list = this.responsedata.TaxSegmentCustomer_list;

  });
}
GetTaxSegment (taxsegment_gid: any){
  debugger
  let params = { taxsegment_gid:taxsegment_gid };
  var othersegmentapi = 'SmrMstTaxSegment/GetTaxSegmentDropDown';
  this.service.getparams(othersegmentapi, params).subscribe((apiresponse: any) =>{
  this.GetTaxSegmentDropDown_list = apiresponse.GetTaxSegmentDropDown_list;
  });
}
isAllSelected1() {
  const numSelected = this.selection1.selected.length;
  const numRows = this.GetotherSegment_list.length;
  return numSelected === numRows;
}
masterToggle1() {
  this.isAllSelected1() ?
    this.selection1.clear() :
    this.GetotherSegment_list.forEach((row: IMapTaxsegment) => this.selection1.select(row));
}



onSubmit (){
  debugger
    const taxsegment_gid = this.taxsegmentform.value.taxsegment_gid;
    
  
    this.pick = this.selection1.selected;
    let customerGids = this.pick.map(item => item.customer_gid);

// Prepare request object
this.CurObj1.GetotherSegment_list = this.pick;
this.CurObj1.taxsegment_gid = taxsegment_gid;
this.CurObj1.customer_gid1 = customerGids;
    
      let list = this.pick;
      this.CurObj1.GetotherSegment_list = list;
   {
      if (this.CurObj1.GetotherSegment_list.length !== 0) {
        console.log('uiu',this.CurObj1);
        this.NgxSpinnerService.show();
        var url1 = 'SmrMstTaxSegment/PostTaxsegmentMoveOn';
       
       
        console.log(this.customer_gid1)
        this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {
          if (result.status === false) {
            this.NgxSpinnerService.hide();
            this.selection1.clear();
            window.scrollTo({
              top: 0,
              behavior: 'smooth'
          });
                
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.selection1.clear();
            this.totalcount();
            window.scrollTo({
              top: 0,
              behavior: 'smooth'
          });
          }
  
          
        });
  
        this.NgxSpinnerService.hide();
        this.selection1.clear();
        
      } else {
        this.ToastrService.warning("Kindly Select At Least One Record to Move Product!");
        window.scrollTo({
          top: 0,
          behavior: 'smooth'
      });
      }
    }
  }



}
