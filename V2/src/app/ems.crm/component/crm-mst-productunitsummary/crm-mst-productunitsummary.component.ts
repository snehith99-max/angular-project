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
  productuomclass_code: any;
  productuomclassedit_name: string;
  productuomclassedit_code: any;
  productuomclassadd_name: string;
  productuomclassadd_code: string;
  batch_flag: any;

}

@Component({
  selector: 'app-crm-mst-productunitsummary',
  templateUrl: './crm-mst-productunitsummary.component.html',
  styleUrls: ['./crm-mst-productunitsummary.component.scss'],
})
export class CrmMstProductunitsummaryComponent {
  reactiveFormReset!: FormGroup;
  reactiveForm!: FormGroup;
  responsedata: any;
  Mktproductunits_list: any;
  productunit!: IProductUnit;
  reactiveFormEdit: FormGroup | any;
  reactiveFormadd: FormGroup | any;
  parameterValue1: any;
  productuomclass_gid: any;
  Mktproductunitgrid_list: any;
  ProductunitFormEdit!: FormGroup;
  productunitedit_data: any;
  myModalUpdateimage9:boolean=false;
  showOptionsDivId: any;
  constructor(
    private formBuilder: FormBuilder,
    private route: Router,
    private ToastrService: ToastrService,
    public service: SocketService,
    public NgxSpinnerService: NgxSpinnerService
  ) {
    this.productunit = {} as IProductUnit;
    // this.reactiveFormadd = new FormGroup({
    //   conversion_rate: new FormControl(''),
    //   sequence_level: new FormControl(''),
    //   productuomclassadd_name: new FormControl(''),
    //   productuomclassadd_code: new FormControl(''),
    //   batch_flag: new FormControl('Y',Validators.required),
    //   productuomclass_gid: new FormControl(''),
    //   productuomclass_name: new FormControl(''),
    // })
  }


  parameterValue: any;
  productunitdata:any;
  parameterValue2:any;
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
        [Validators.required,]

      ),
      productuomclass_code: new FormControl(''),
    });

    ///form values for edit///
    this.reactiveFormEdit = new FormGroup({
      productuomclassedit_name: new FormControl(
        this.productunit.productuomclassedit_name,
        [Validators.required,]
      ),
      productuomclassedit_code: new FormControl(
        this.productunit.productuomclassedit_code,
        [Validators.required]
      ),
      productuomclass_gid: new FormControl(''),
    });

    this.reactiveFormadd = new FormGroup({
      productuomclassadd_name: new FormControl(
        this.productunit.productuomclassadd_name,
        [Validators.required, ]
      ),
      productuomclassadd_code: new FormControl(''),
      sequence_level: new FormControl(this.productunit.sequence_level, [
        Validators.required,
      ]),
      conversion_rate: new FormControl(this.productunit.conversion_rate, [
        Validators.required,
      ]),
      productuomclass_gid: new FormControl(''),

      batch_flag: new FormControl('Y', [
        Validators.required,]),

      productuomclass_name: new FormControl(''),
    });
    this.ProductunitFormEdit = new FormGroup({

      productuomedit_name: new FormControl(''),
      productuomedit_code: new FormControl(''),
      sequence_leveledit: new FormControl(''),
      conversion_rateedit: new FormControl(''),
      productuomclass_gid: new FormControl(''),
      batch_flagedit: new FormControl(''),
      productuomclass_nameedit: new FormControl(''),
      productuom_gid:new FormControl('')
    })
  }

  toggleOptions(callresponse_gid: any) {
    if (this.showOptionsDivId === callresponse_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = callresponse_gid;
    }
  }


  ////////////Get Summary for product unit//////////////////////
  GetProductunitSummary() {
    var url = 'ProductUnits/GetMktProductUnitSummary';
    this.service.get(url).subscribe((result: any) => {
      $('#Mktproductunits_list').DataTable().destroy();
      this.responsedata = result;
      this.Mktproductunits_list = this.responsedata.Mktproductunits_list;
      setTimeout(() => {
        $('#Mktproductunits_list').DataTable();
      }, 1);
    });
  }

  /////////For Add PopUp/////////
  get productuomclass_name() {
    return this.reactiveForm.get('productuomclass_name')!;
  }
  get productuomclass_code() {
    return this.reactiveForm.get('productuomclass_code')!;
  }
  Openproductunit(productunitedit_data: string) {
    //console.log('ee',productunitedit_data)
    this.productunitdata=productunitedit_data;
    this.ProductunitFormEdit.get('productuomedit_code')?.setValue(this.productunitdata.productuom_code);
    this.ProductunitFormEdit.get('productuomedit_name')?.setValue(this.productunitdata.productuom_name);
    this.ProductunitFormEdit.get('sequence_leveledit')?.setValue(this.productunitdata.sequence_level);
    this.ProductunitFormEdit.get('conversion_rateedit')?.setValue(this.productunitdata.convertion_rate);
    this.ProductunitFormEdit.get('batch_flagedit')?.setValue(this.productunitdata.baseuom_flag);
    this.ProductunitFormEdit.get('productuom_gid')?.setValue(this.productunitdata.productuom_gid);
    
    //console.log('ee', this.ProductunitFormEdit.get('productuomedit_name')?.setValue(this.productunitedit_data.productuom_name))
  }
  get productuomedit_name() {
    return this.ProductunitFormEdit.get('productuomedit_name')!;
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
  onsubmitproductunitedit() {
    if (
      this.ProductunitFormEdit.value.productuomedit_name != null
    ) {
      this.ProductunitFormEdit.value;
      var url = 'ProductUnits/EditMktProductunits';
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
      });
    } else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
    }
  }

  onsubmit() {
    if (
      this.reactiveForm.value.productuomclass_name != null
    ) {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'ProductUnits/PostMktProductUnit';
      this.NgxSpinnerService.show();
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
          this.GetProductunitSummary();
        } else {
          this.reactiveForm.get('productuomclass_name')?.setValue(null);
          this.reactiveForm.get('productuomclass_code')?.setValue(null);
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

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter;
    this.reactiveFormEdit.get('productuomclassedit_code')?.setValue(this.parameterValue1.productuomclass_code);
    this.reactiveFormEdit
      .get('productuomclassedit_name')
      ?.setValue(this.parameterValue1.productuomclass_name);
    this.reactiveFormEdit
      .get('productuomclass_gid')
      ?.setValue(this.parameterValue1.productuomclass_gid);
  }

  // add///

  get productuomclassadd_code() {
    return this.reactiveFormadd.get('productuomclassadd_code')!;
  }
  get productuomclassadd_name() {
    return this.reactiveFormadd.get('productuomclassadd_name')!;
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

  ////////////Update popup////////
  public onupdate(): void {
    
    if (
      this.reactiveFormEdit.value.productuomclassedit_name != null &&
      this.reactiveFormEdit.value.productuomclassedit_code != null
    ) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      var url = 'ProductUnits/UpdatedMktProductunit';
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
    this.NgxSpinnerService.show();
    var url = 'ProductUnits/deleteMktProductunitSummary';
    let param = {
      productuomclass_gid: this.parameterValue,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.NgxSpinnerService.hide();
      this.GetProductunitSummary();
    });
  }
  openeditdelete(productuom_gid:string){
    this.parameterValue2 = productuom_gid;
  }
  ondelete2(){
    this.NgxSpinnerService.show();
    var url = 'ProductUnits/DeletproductuniteMktsummary';
    let param = {
      productuom_gid: this.parameterValue2
    };
   
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.NgxSpinnerService.hide();
     
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
      productuomclassadd_name: this.reactiveFormadd.value.productuomclassadd_name,
      sequence_level: this.reactiveFormadd.value.sequence_level,
      productuomclass_name: this.reactiveFormadd.value.productuomclass_name,
      batch_flag: this.reactiveFormadd.value.batch_flag
    }
    var url = 'ProductUnits/PostMktProductunits'
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
    
   this.myModalUpdateimage9=true;
    this.reactiveFormadd.get("productuomclass_gid")?.setValue(productuomclass_gid);
    var url = 'ProductUnits/GetMktProductUnitSummarygrid'
    let param = {
      productuomclass_gid: productuomclass_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.Mktproductunitgrid_list = this.responsedata.Mktproductunitgrid_list;
      this.productunit.productuomclass_name = this.Mktproductunitgrid_list[0].productuomclass_name;
      //console.log( this.Mktproductunitgrid_list)

    });
  }


  onClose() {

    this.route.navigate(['/smr/GetMktProductUnitSummary']);
  }

  productunitclass(productuomclass_gid: any) {
    this.reactiveFormadd.get("productuomclass_gid")?.setValue(productuomclass_gid);
    var url = 'ProductUnits/GetMktProductunits'

    let param = {
      productuomclass_gid: productuomclass_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.Mktproductunitgrid_list = result.Mktproductunitgrid_list;
      this.responsedata = result;


      this.reactiveFormadd.get("productuomclass_name")?.setValue(this.Mktproductunitgrid_list[0].productuomclass_name);


    });
  }

  onclose1() {
    this.reactiveFormadd.reset();
    this.ProductunitFormEdit.reset();
  }
}
