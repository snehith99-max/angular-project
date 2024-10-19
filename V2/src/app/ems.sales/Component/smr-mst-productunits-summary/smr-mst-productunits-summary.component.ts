import { Component } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';

interface IProductUnit {
  conversion_rate: any;
  sequence_level: any;
  productuomclass_name: string;
  productuom_name: string;
  productuomclass_code: any;
  productuomclassedit_name: string;
  productuomclassedit_code: any;
  productunit_name:any;
  productuomclassedit_name1: string;
  productuomclassedit_code1: string;
  batch_flag:any;
  productuomclass_nameedit:string;
  

}

@Component({
  selector: 'app-smr-mst-productunits-summary',
  templateUrl: './smr-mst-productunits-summary.component.html',
  styleUrls: ['./smr-mst-productunits-summary.component.scss'],
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
export class SmrMstProductunitsSummaryComponent {
  reactiveFormReset!: FormGroup;
  reactiveForm!: FormGroup;
  responsedata: any;
  salesproductunit_list: any;
  productunit!: IProductUnit;
  reactiveFormEdit: FormGroup | any;
  reactiveFormadd: FormGroup | any;
  parameterValue1: any;
  productuomclass_gid: any;

  salesproductunitgrid_list: any;
  ProductunitFormEdit: FormGroup | any;
  unitclass_list: any[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private route: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService
    ) {
    this.productunit = {} as IProductUnit;
    this.reactiveFormadd = new FormGroup({
      conversion_rate: new FormControl(''),
      sequence_level: new FormControl(''),
      productuomclassedit_name1: new FormControl(''),
      productuomclassedit_code1: new FormControl(''),
      batch_flag: new FormControl(''),
      productuomclass_gid: new FormControl(''),
      productuomclass_name: new FormControl('',[Validators.required]),
    })
  }


  parameterValue: any;
  productunitdata:any;
  parameterValue2:any;
  showOptionsDivId: any; 

  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });

    this.GetProductunitSummary();

    ///form values for add popup///
    this.reactiveForm = new FormGroup({
      productuomclass_name: new FormControl(
        this.productunit.productuomclass_name,
        [Validators.required]
      ),
      productuom_name: new FormControl(
        this.productunit.productuom_name,
        [Validators.required]
      ),
      productuomclass_code: new FormControl(
        this.productunit.productuomclass_code,
        [Validators.required]
      ),
    });

    ///form values for edit///
    this.reactiveFormEdit = new FormGroup({
      productuomclassedit_name: new FormControl(
        this.productunit.productuomclassedit_name,
        [Validators.required]
      ),
      productuomclassedit_code: new FormControl(
        this.productunit.productuomclassedit_code,
        [Validators.required]
      ),
      productunit_name: new FormControl(
        this.productunit.productunit_name,
        [Validators.required]
      ),
      productuomclass_gid: new FormControl(''),
    });

    this.reactiveFormadd = new FormGroup({
      productuomclassedit_name1: new FormControl(
        this.productunit.productuomclassedit_name1,
        [Validators.required]
      ),
      productuomclassedit_code1: new FormControl(''),
      sequence_level: new FormControl(this.productunit.sequence_level, [
        Validators.required,
      ]),
      conversion_rate: new FormControl(this.productunit.conversion_rate, [
        Validators.required,
      ]),
      productuomclass_gid: new FormControl(''),

      batch_flag: new FormControl('N', [
        Validators.required,]),
        
      productuomclass_name: new FormControl('',[Validators.required]),
    });


    // form control for edit product unit class

    this.ProductunitFormEdit = new FormGroup({

      productuomedit_name: new FormControl(''),
      productuomedit_code: new FormControl('',[Validators.required]),
      sequence_leveledit: new FormControl(''),
      conversion_rateedit: new FormControl(''),
      productuomclass_gid: new FormControl(''),
      batch_flagedit: new FormControl(''),
      productuomclass_nameedit: new FormControl(this.productunit.productuomclass_nameedit, [
        Validators.required,
      ]),
      productuom_gid:new FormControl('')
    });

    var api2 = 'SmrMstProductUnit/Getproductunitclassdropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.unitclass_list = result.unitclass_list;
    });

  }

  ////////////Get Summary for product unit//////////////////////
  GetProductunitSummary() {
    var url = 'SmrMstProductUnit/GetSalesProductUnitSummary';
    this.service.get(url).subscribe((result: any) => {
      $('#salesproductunit_list').DataTable().destroy();
      this.responsedata = result;
      this.salesproductunit_list = this.responsedata.salesproductunit_list;
      setTimeout(() => {
        $('#salesproductunit_list').DataTable();
      }, 1);
    });
  }

  /////////For Add PopUp/////////
  get productuomclass_name() {
    return this.reactiveForm.get('productuomclass_name')!;
  }
  get productuomedit_code() {
    return this.ProductunitFormEdit.get('productuomedit_code')!;
  }
  get productuom_name() {
    return this.reactiveForm.get('productuom_name')!;
  }
  get productuomclass_code() {
    return this.reactiveForm.get('productuomclass_code')!;
  }
  get productunit_name() {
    return this.reactiveFormEdit.get('productunit_name')!;
  }

  onsubmit() {
    if (
      this.reactiveForm.value.productuomclass_name != null) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'SmrMstProductUnit/PostProductunits';
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.reactiveForm.value)
        .subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.GetProductunitSummary();
            this.reactiveForm.reset();
          } else {
            
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.reactiveForm.reset();

            this.GetProductunitSummary();
          }
        });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }

  /////EDIT POPUP/////
  get productuomclassedit_code() {
    return this.reactiveFormEdit.get('productuomclassedit_code')!;
  }
  get productuomclassedit_name() {
    return this.reactiveFormEdit.get('productuomclassedit_name')!;
  }

 

  // add///

  get productuomclassedit_code1() {
    return this.reactiveFormadd.get('productuomclassedit_code1')!;
  }
  get productuomclassedit_name1() {
    return this.reactiveFormadd.get('productuomclassedit_name1')!;
  }
  get sequence_level() {
    return this.reactiveFormadd.get('sequence_level')!;
  }
  get batch_flag() {
    return this.reactiveFormadd.get('batch_flag')!;
  }
  get conversion_rate() {
    return this.reactiveFormadd.get('conversion_rate')!;
  }
  get productuomedit_name() {
    return this.ProductunitFormEdit.get('productuomedit_name')!;
  }
  get productuomclass_nameedit() {
    return this.ProductunitFormEdit.get('productuomclass_nameedit')!;
  }
  get sequence_leveledit() {
    return this.ProductunitFormEdit.get('sequence_leveledit')!;
  }
  get conversion_rateedit() {
    return this.ProductunitFormEdit.get('conversion_rateedit')!;
  } 
  get batch_flagedit() {
    return this.ProductunitFormEdit.get('batch_flagedit')!;
  }

  ////////////Update popup////////
  public onupdate(): void {
    if (
      this.reactiveFormEdit.value.productuomclassedit_name != null &&  this.reactiveFormEdit.value.productuomclassedit_code != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      var url = 'SmrMstProductUnit/UpdatedSalesProductunit';
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.reactiveFormEdit.value)
        .pipe()
        .subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.GetProductunitSummary();
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.GetProductunitSummary();
          }
        });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  ////////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter;
  }
  ondelete() {

    var url = 'SmrMstProductUnit/deleteSalesProductunitSummary';
    let param = {
      productuom_gid: this.parameterValue,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.GetProductunitSummary();
    });
  }
  onclose() {
    this.reactiveForm.reset();
  }



  //// ADD PRODUCT UNIT ////
  onSubmitprod() {

    var params = {
      productuomclass_gid: this.reactiveFormadd.value.productuomclass_gid,
      conversion_rate: this.reactiveFormadd.value.conversion_rate,
      productuomclassedit_name1: this.reactiveFormadd.value.productuomclassedit_name1,
      sequence_level: this.reactiveFormadd.value.sequence_level,
      productuomclass_name: this.reactiveFormadd.value.productuomclass_name,
      batch_flag : this.reactiveFormadd.value.batch_flag
    }
    var url = 'SmrMstProductUnit/PostProductunits'
    this.NgxSpinnerService.show();
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
     
      else {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      this.reactiveFormadd.reset();
    });

  }
  Details(productuomclass_gid: any) {
  
    this.reactiveFormadd.get("productuomclass_gid")?.setValue(productuomclass_gid);
    var url = 'SmrMstProductUnit/GetSalesProductUnitSummarygrid'
    let param = {
      productuomclass_gid: productuomclass_gid
    }
      this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.salesproductunitgrid_list =  this.responsedata.salesproductunitgrid_list;    

    });
  }


  onClose() {

    this.route.navigate(['/smr/SmrMstProductunitsSummary']);
  }

  productunitclass(productuomclass_gid: any) {
    this.reactiveFormadd.get("productuomclass_gid")?.setValue(productuomclass_gid);
    var url = 'SmrMstProductUnit/GetProductunits'

    let param = {
      productuomclass_gid: productuomclass_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.salesproductunitgrid_list = result.salesproductunitgrid_list;
      this.responsedata = result; 
      this.reactiveFormadd.get("productuomclass_name")?.setValue(this.salesproductunitgrid_list[0].productuomclass_name);

    });
  }

  onclose1(){
    this.reactiveFormadd.reset();
  }
  

  Openproductunit(data: any) {
    this.productunitdata = data;
    this.ProductunitFormEdit.get('productuomedit_code')?.setValue(this.productunitdata.productuomclass_code);
    this.ProductunitFormEdit.get('productuomedit_name')?.setValue(this.productunitdata.productuom_name);
    this.ProductunitFormEdit.get('productuomclass_gid')?.setValue(this.productunitdata.productuomclass_gid);
    this.ProductunitFormEdit.get('productuomclass_nameedit')?.setValue(this.productunitdata.productuomclass_gid);
    this.ProductunitFormEdit.get('productuom_gid')?.setValue(this.productunitdata.productuom_gid);

  }
  openeditdelete(productuom_gid:string){
    this.parameterValue2 = productuom_gid;
  }
  ondelete2(){
    var url = 'SmrMstProductUnit/Deletproductunitsalessummary';
    let param = {
      productuom_gid: this.parameterValue2
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
     
    });
  }
  onsubmitproductunitedit() {
    if (
      this.ProductunitFormEdit.value.productuomedit_name != null) {

      this.ProductunitFormEdit.value;
      var url = 'SmrMstProductUnit/EditsalesProductunits';
      this.NgxSpinnerService.show();
      this.service.post(url, this.ProductunitFormEdit.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
          this.ProductunitFormEdit.reset();
        } else {
          
          this.ToastrService.success(result.message);
          this.NgxSpinnerService.hide();
          this.ProductunitFormEdit.reset();
        }
        this.GetProductunitSummary();
      });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }
  onclose2() {
    this.ProductunitFormEdit.reset();
  }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
}
