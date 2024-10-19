import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { data } from 'jquery';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-acc-mst-glcode-summary',
  templateUrl: './acc-mst-glcode-summary.component.html',
  styleUrls: ['./acc-mst-glcode-summary.component.scss']
})

export class AccMstGlcodeSummaryComponent {
  reactiveform1: FormGroup | any;
  salesaddForm: FormGroup | any;
  SalesTypeEditForm: FormGroup | any;
  SalesTypeViewForm: FormGroup | any;
  SalesTypeMappingForm: FormGroup | any;
  PurchaseAddForm: FormGroup | any;
  TaxAddForm: FormGroup | any;
  PurchaseEditForm: FormGroup | any;
  PurchaseTypeViewForm: FormGroup | any;
  PurchaseTypeMappingForm: FormGroup | any;
  MapAccountForm: FormGroup | any;
  EditMapAccountForm: FormGroup | any;
  AddMapAccountForm: FormGroup | any;
  DepartmentMapForm: FormGroup | any;
  DebtorExternalCodeForm: FormGroup | any;
  CreditorExternalCodeForm: FormGroup | any;
  TaxExternalGLCodeEdit: FormGroup | any;
  EditTaxForm: FormGroup | any;
  AssetExternalGLCodeEdit: FormGroup | any;
  ExpenseGroupMapForm: FormGroup | any;
  TaxAccountMapForm: FormGroup | any;
  SalaryCompMappingForm: FormGroup | any;
  responsedata: any;
  DebtorSummary_List: any;
  CreditorSummary_List: any;
  SalesTypeSummary_List: any;
  PurchaseTypeSummary_List: any;
  DepartmentSummary_List: any;
  AcctMappingSummary_List: any;
  ExpenseGroupSummary_List: any;
  parameterValue1: any;
  salestype_codeview: any;
  parameterValue2: any;
  taxsegmentmappinglist:any;
  salestype_gid: any;
  viewsalestype_code: any;
  viewsalestype_name: any;
  MappingAcctTo_List: any;
  parameterValue3: any;
  mappingsalestype_code: any;
  mappingsalestype_name: any;
  parameterValue4: any;
  purchasetype_codeview: any;
  parameterValue5: any;
  viewpurchasetype_code: any;
  viewpurchasetype_name: any;
  PurchaseMappingAcctTo_List: any;
  parameterValue6: any;
  mappingpurchasetype_code: any;
  mappingpurchasetype_name: any;
  TaxPayableSummary_List: any;
  AssetDtlsSummary_List: any;
  parameterValue7: any;
  taxgl_code: any;
  assetgl_code: any;
  parameterValue8: any;
  parameterValue9: any;
  empaccount_code: any;
  empaccount_name: any;
  empgl_code: any;
  externalassetacct_name: any;
  asset_name: any;
  ExpenseMappingAcct_List: any;
  parameterValue10: any;
  expensegroup_code: any;
  expensegroup_name: any;
  AccountMapping_List: any;
  parameterValue11: any;
  editscreen_name: any;
  editfield_name: any;
  editmodule_name: any;
  parameterValue12: any;
  debtoracct_code: any;
  debtoracct_name: any;
  debtorgl_code: any;
  parameterValue13: any;
  creditorvendor_code: any;
  creditorcompany_name: any;
  creditorgl_code: any;
  showOptionsDivId: any;
  taxsegmentaddForm: FormGroup | any;
  taxsegmentsummary_list: any;
  taxsegmentViewForm: FormGroup | any;
  taxsegment_gid: any;
  viewtaxsegment_name: any;
  viewtaxsegment_code: any;
  parameterValue14: any;
  taxsegmentEditForm: FormGroup | any;
  taxsegmentMappingForm: FormGroup | any;
  viewreference_type: any;
  parameterValue15: any;
  mappingtaxsegment_name: any;
  mappingtaxsegment_reftype: any;
  TaxSegmentAccountMappingTo_List: any;
  taxpercentage: any;
  taxname: any;
  GetTaxAccountMappingTo_List: any;
  tax_gid: any;
  department_prefix: any;
  department_code: any;
  department_name: any;
  GetDepartmentAccountDropDown_list: any;
  GetSalaryCompMappingDropDown_list: any;
  GetSalaryComponentSummary_list: any;
  salarycomponent_gid1: any;
  componentgroup_name: any;
  component_code: any;
  component_name: any;

  constructor(public service: SocketService, private router: ActivatedRoute, private route: Router,
    private FormBuilder: FormBuilder, private NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService, private datePipe: DatePipe,) {
  }

  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',
    };
    flatpickr('.date-picker', options);

    this.reactiveform1 = new FormGroup({
      // from_date: new FormControl(''),
      // to_date: new FormControl(''),
    });

    this.salesaddForm = new FormGroup({
      salestype_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      salestype_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    });

    this.SalesTypeEditForm = new FormGroup({
      editsalestype_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      salestype_codeview: new FormControl(null),
      salestype_gid: new FormControl(null),
    });

    this.SalesTypeViewForm = new FormGroup({
      viewsalestype_code: new FormControl(null),
      viewsalestype_name: new FormControl(null),
      salestype_gid: new FormControl(null),
    });

    this.SalesTypeMappingForm = new FormGroup({
      account_name: new FormControl(null, Validators.required),
      mappingsalestype_code: new FormControl(null),
      mappingsalestype_name: new FormControl(null),
      salestype_gid: new FormControl(null),
    });

    this.PurchaseAddForm = new FormGroup({
      purchasetype_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      purchasetype_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    });

    this.PurchaseEditForm = new FormGroup({
      editpurchasetype_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      purchasetype_codeview: new FormControl(null),
      purchasetype_gid: new FormControl(null),
    });

    this.PurchaseTypeViewForm = new FormGroup({
      viewpurchasetype_code: new FormControl(null),
      viewpurchasetype_name: new FormControl(null),
      purchasetype_gid: new FormControl(null),
    });

    this.PurchaseTypeMappingForm = new FormGroup({
      account_name: new FormControl(null, Validators.required),
      mappingpurchasetype_code: new FormControl(null),
      mappingpurchasetype_name: new FormControl(null),
      purchasetype_gid: new FormControl(null),
    });

    this.TaxAddForm = new FormGroup({
      tax_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      tax_percentage: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    });

    this.EditMapAccountForm = new FormGroup({
      editmapping_account: new FormControl(null, Validators.required),
      accountmapping_gid: new FormControl(null),
    });

    this.AddMapAccountForm = new FormGroup({
      module_name: new FormControl(null, Validators.required),
      screen_name: new FormControl(null, Validators.required),
      field_name: new FormControl(null, Validators.required),
      mapping_account: new FormControl(null, Validators.required),
    });

    this.ExpenseGroupMapForm = new FormGroup({
      exaccount_name: new FormControl(null, Validators.required),
      expensegroup_code: new FormControl(null),
      expensegroup_name: new FormControl(null),
      producttype_gid: new FormControl(null),
    });

    this.DepartmentMapForm = new FormGroup({
      Dept_mapping_account: new FormControl(null, Validators.required),
      department_gid: new FormControl(null)
    });

    this.TaxExternalGLCodeEdit = new FormGroup({
      externalacct_name: new FormControl(null),
      externalgl_code: new FormControl(null, Validators.required),
      tax_gid: new FormControl(null),
      taxgl_code: new FormControl(null),
    });

    this.AssetExternalGLCodeEdit = new FormGroup({
      assetexternalacct_name: new FormControl(null),
      externalgl_code: new FormControl(null),
      assetdtl_gid: new FormControl(null),
      assetexternalgl_code: new FormControl(null, Validators.required),
    });

    this.EditTaxForm = new FormGroup({
      edittax_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      edittax_percentage: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      tax_gid: new FormControl(null),
    });

    this.DebtorExternalCodeForm = new FormGroup({
      customer_gid: new FormControl(null),
      debtorexternalgl_code: new FormControl('', Validators.required),
      debtorrgl_code: new FormControl(null)
    });

    this.CreditorExternalCodeForm = new FormGroup({
      vendor_gid: new FormControl(null),
      creditorexternalgl_code: new FormControl('', Validators.required),
      creditorgl_code: new FormControl(null)
    });

    this.taxsegmentaddForm = new FormGroup({
      taxsegment_code: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      taxsegment_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
    });

    this.taxsegmentViewForm = new FormGroup({
      viewtaxsegment_code: new FormControl(null),
      viewtaxsegment_name: new FormControl(null),
      taxsegment_gid: new FormControl(null),
    });

    this.taxsegmentEditForm = new FormGroup({
      edittaxsegment_name: new FormControl('', [Validators.required, Validators.pattern(/^\S.*$/)]),
      edittaxsegment_code: new FormControl(null),
      taxsegment_gid: new FormControl(null),
    });

    this.taxsegmentMappingForm = new FormGroup({
      taxaccount_name: new FormControl(null, Validators.required),
      mappingtaxsegment_name: new FormControl(null),
      mappingtaxsegment_reftype: new FormControl(null),
      taxsegment_gid: new FormControl(null),
    });
    this.TaxAccountMapForm = new FormGroup({
      account_gid : new FormControl(null)
    });
    this.SalaryCompMappingForm = new FormGroup({
      salarycomponent_ledger_gid : new FormControl(null)
    });

    this.AccTrnDebtorSummary();
    this.AccTrnCreditorSummary();
    this.AccTrnTaxPayableSummary();
    this.AccTrnAssetDtlsSummary();
    this.AccTrnSalesTypeSummary();
    this.AccTrnPurchaseTypeSummary();
    this.AccTrnEmployeeSummary();
    this.AccTrnAcctMappingSummary();
    this.AccTrnExpenseGroupSummary();
    this.SalaryComponentSummary();

    var url = 'GLCode/GetMappingAccountTo'
    this.service.get(url).subscribe((result: any) => {
      this.MappingAcctTo_List = result.GetMappingAcctTo_List;
    });

    var url = 'GLCode/GetPurchaseMappingAccountTo'
    this.service.get(url).subscribe((result: any) => {
      this.PurchaseMappingAcctTo_List = result.GetPurchaseMappingAcctTo_List;
    });

    var url = 'GLCode/GetExpenseMappingAcct'
    this.service.get(url).subscribe((result: any) => {
      this.ExpenseMappingAcct_List = result.GetExpenseMappingAcct_List;
    });

    var url = 'GLCode/GetAccountMappingDtl'
    this.service.get(url).subscribe((result: any) => {
      this.AccountMapping_List = result.GetAccountMapping_List;
    });
    
    var url = 'GLCode/GetTaxSegmentMapping'
    this.service.get(url).subscribe((result: any) => {
      this.taxsegmentmappinglist = result.taxsegmentmapping_list;
    });

    var url = 'GLCode/GetTaxSegmentAccountMappingTo'
    this.service.get(url).subscribe((result: any) => {
      this.TaxSegmentAccountMappingTo_List = result.GetTaxSegmentAccountMappingTo_List;
    });
    var url = 'GLCode/GetTaxAccountMappingTo'
    this.service.get(url).subscribe((result: any) => {
      this.GetTaxAccountMappingTo_List = result.GetTaxAccountMappingTo_List;
    });

    var url = 'GLCode/GetDeptAcctMappingDropdown';
    this.service.get(url).subscribe((result: any) => {
      this.GetDepartmentAccountDropDown_list = result.GetDepartmentAccountDropDown_list
    });
    var url = 'GLCode/GetSalaryCompMappingDropdown';
    this.service.get(url).subscribe((result: any) => {
      this.GetSalaryCompMappingDropDown_list = result.GetSalaryCompDropdown_list
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }

  AccTrnDebtorSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetDebtorSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#DebtorSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.DebtorSummary_List = result.GetDebtorSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#DebtorSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  

  AccTrnCreditorSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetCreditorSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#CreditorSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.CreditorSummary_List = result.GetCreditorSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#CreditorSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  taxsegmentmapping(){
    var url = 'GLCode/GetTaxSegmentMapping'
    this.service.get(url).subscribe((result: any) => {
      this.taxsegmentmappinglist = result.taxsegmentmapping_list;
    });

  }

  AccTrnTaxPayableSummary() {
    debugger
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetTaxPayableSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#TaxPayableSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.TaxPayableSummary_List = result.GetTaxPayableSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#TaxPayableSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnAssetDtlsSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetAssetDtlsSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#AssetDtlsSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.AssetDtlsSummary_List = result.GetAssetDtlsSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#AssetDtlsSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnSalesTypeSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetSalesTypeSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#SalesTypeSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.SalesTypeSummary_List = result.GetSalesTypeSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#SalesTypeSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnPurchaseTypeSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetPurchaseTypeSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#PurchaseTypeSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.PurchaseTypeSummary_List = result.GetPurchaseTypeSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#PurchaseTypeSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnEmployeeSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetDepartmentSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#DepartmentSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.DepartmentSummary_List = result.GetDepartmentSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#DepartmentSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnAcctMappingSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetAcctMappingSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#AcctMappingSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.AcctMappingSummary_List = result.GetAcctMappingSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#AcctMappingSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  AccTrnExpenseGroupSummary() {
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetExpenseGroupSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#ExpenseGroupSummary_List').DataTable().destroy();
      this.responsedata = result;
      this.ExpenseGroupSummary_List = result.GetExpenseGroupSummary_List;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#ExpenseGroupSummary_List').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  SalaryComponentSummary(){
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetSalaryCompSummary';
    this.service.get(url).subscribe((result: any) => {
      $('#GetSalaryComponentSummary_list').DataTable().destroy();
      this.GetSalaryComponentSummary_list = result.GetSalaryCompSummary_list
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#GetSalaryComponentSummary_list').DataTable(
          {
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }

  get salestype_code() {
    return this.salesaddForm.get('salestype_code')!;
  }

  get salestype_name() {
    return this.salesaddForm.get('salestype_name')!;
  }

  get editsalestype_name() {
    return this.SalesTypeEditForm.get('editsalestype_name')!;
  }

  get account_name() {
    return this.SalesTypeMappingForm.get('account_name')!;
  }

  get purchasetype_code() {
    return this.PurchaseAddForm.get('purchasetype_code')!;
  }

  get purchasetype_name() {
    return this.PurchaseAddForm.get('purchasetype_name')!;
  }

  get editpurchasetype_name() {
    return this.PurchaseEditForm.get('editpurchasetype_name')!;
  }

  get tax_name() {
    return this.TaxAddForm.get('tax_name')!;
  }

  get tax_percentage() {
    return this.TaxAddForm.get('tax_percentage')!;
  }

  get edittax_name() {
    return this.EditTaxForm.get('edittax_name')!;
  }

  get edittax_percentage() {
    return this.EditTaxForm.get('edittax_percentage')!;
  }

  get externalgl_code() {
    return this.TaxExternalGLCodeEdit.get('externalgl_code')!;
  }

  get assetexternalgl_code() {
    return this.AssetExternalGLCodeEdit.get('assetexternalgl_code')!;
  }

  get Dept_mapping_account() {
    return this.DepartmentMapForm.get('Dept_mapping_account')!;
  }

  get exaccount_name() {
    return this.ExpenseGroupMapForm.get('exaccount_name')!;
  }

  get module_name() {
    return this.AddMapAccountForm.get('module_name')!;
  }

  get screen_name() {
    return this.AddMapAccountForm.get('screen_name')!;
  }

  get field_name() {
    return this.AddMapAccountForm.get('field_name')!;
  }

  get mapping_account() {
    return this.AddMapAccountForm.get('mapping_account')!;
  }

  get editmapping_account() {
    return this.EditMapAccountForm.get('editmapping_account')!;
  }

  get debtorexternalgl_code() {
    return this.DebtorExternalCodeForm.get('debtorexternalgl_code')!;
  }

  get creditorexternalgl_code() {
    return this.CreditorExternalCodeForm.get('creditorexternalgl_code')!;
  }

  get taxsegment_code() {
    return this.taxsegmentaddForm.get('taxsegment_code')!;
  }

  get taxsegment_name() {
    return this.taxsegmentaddForm.get('taxsegment_name')!;
  }

  get edittaxsegment_name() {
    return this.taxsegmentEditForm.get('edittaxsegment_name')!;
  }

  get taxaccount_name() {
    return this.taxsegmentMappingForm.get('taxaccount_name')!;
  }

  get account_gid() {
    return this.TaxAccountMapForm.get('account_gid')!;
  }
  get salarycomponent_ledger_gid() {
    return this.SalaryCompMappingForm.get('salarycomponent_ledger_gid')!;
  }

  // Sales Type Tab Functionality
  Submit_Salestype() {
    this.salesaddForm.value;
    if (this.salesaddForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'GLCode/PostSalesType';
      this.service.post(url, this.salesaddForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.salesaddForm.get("salestype_code")?.setValue(null);
          this.salesaddForm.get("salestype_name")?.setValue(null);
          this.salesaddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.salesaddForm.get("salestype_code")?.setValue(null);
          this.salesaddForm.get("salestype_name")?.setValue(null);
          this.salesaddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }
  }

  Edit_Salestype(parameter: any) {
    this.parameterValue1 = parameter
    console.log(this.parameterValue1, 'this.parameterValue1');
    this.SalesTypeEditForm.get("salestype_gid")?.setValue(this.parameterValue1.salestype_gid);
    this.SalesTypeEditForm.get("editsalestype_name")?.setValue(this.parameterValue1.salestype_name);
    this.salestype_codeview = this.parameterValue1.salestype_code;
  }

  Update_Salestype() {
    this.SalesTypeEditForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetupdateSalesTypeDtls';
    this.service.post(url, this.SalesTypeEditForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.SalesTypeEditForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.SalesTypeEditForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  View_Salestype(parameter: any) {
    this.parameterValue2 = parameter
    console.log(this.parameterValue2, 'this.parameterValue2');
    this.salestype_gid = this.parameterValue2.salestype_gid;
    this.viewsalestype_code = this.parameterValue2.salestype_code;
    this.viewsalestype_name = this.parameterValue2.salestype_name;
  }

  Sales_Mapping(parameter: any) {
    this.parameterValue3 = parameter
    console.log(this.parameterValue3, 'this.parameterValue3');
    this.SalesTypeMappingForm.get("salestype_gid")?.setValue(this.parameterValue3.salestype_gid);
    this.SalesTypeMappingForm.get("account_name")?.setValue(this.parameterValue3.account_gid);
    this.mappingsalestype_code = this.parameterValue3.salestype_code;
    this.mappingsalestype_name = this.parameterValue3.salestype_name;
  }

  Update_SalestypeMapping() {
    this.SalesTypeMappingForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateSalesTypeMapping';
    this.service.post(url, this.SalesTypeMappingForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.SalesTypeMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.SalesTypeMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  // purchase Type Tab Functionality
  Submit_Purchasetype() {
    this.PurchaseAddForm.value;
    if (this.PurchaseAddForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'GLCode/PostPurchaseType';
      this.service.post(url, this.PurchaseAddForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.PurchaseAddForm.get("purchasetype_code")?.setValue(null);
          this.PurchaseAddForm.get("purchasetype_name")?.setValue(null);
          this.PurchaseAddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.PurchaseAddForm.get("purchasetype_code")?.setValue(null);
          this.PurchaseAddForm.get("purchasetype_name")?.setValue(null);
          this.PurchaseAddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }
  }

  Edit_Purchasetype(parameter: any) {
    this.parameterValue4 = parameter
    console.log(this.parameterValue4, 'this.parameterValue1');
    this.PurchaseEditForm.get("purchasetype_gid")?.setValue(this.parameterValue4.purchasetype_gid);
    this.PurchaseEditForm.get("editpurchasetype_name")?.setValue(this.parameterValue4.purchasetype_name);
    this.purchasetype_codeview = this.parameterValue4.purchasetype_code;
  }

  Update_Purchasetype() {
    this.PurchaseEditForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetupdatePurchaseTypeDtls';
    this.service.post(url, this.PurchaseEditForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.PurchaseEditForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.PurchaseEditForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  View_Purchasetype(parameter: any) {
    this.parameterValue5 = parameter
    console.log(this.parameterValue5, 'this.parameterValue5');
    this.salestype_gid = this.parameterValue5.salestype_gid;
    this.viewpurchasetype_code = this.parameterValue5.purchasetype_code;
    this.viewpurchasetype_name = this.parameterValue5.purchasetype_name;
  }

  Purchase_Mapping(parameter: any) {
    this.parameterValue6 = parameter
    console.log(this.parameterValue6, 'this.parameterValue6');
    this.PurchaseTypeMappingForm.get("purchasetype_gid")?.setValue(this.parameterValue6.purchasetype_gid);
    this.PurchaseTypeMappingForm.get("account_name")?.setValue(this.parameterValue6.account_gid);
    this.mappingpurchasetype_code = this.parameterValue6.purchasetype_code;
    this.mappingpurchasetype_name = this.parameterValue6.purchasetype_name;
  }

  Update_PurchasetypeMapping() {
    this.PurchaseTypeMappingForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdatePurchaseTypeMapping';
    this.service.post(url, this.PurchaseTypeMappingForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.PurchaseTypeMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.PurchaseTypeMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  // Tax Payable Tab Functionality
  Submit_Tax() {
    this.TaxAddForm.value;
    if (this.TaxAddForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'GLCode/PostTaxPayables';
      this.service.post(url, this.TaxAddForm.value).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.TaxAddForm.get("tax_name")?.setValue(null);
          this.TaxAddForm.get("tax_percentage")?.setValue(null);
          this.TaxAddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.TaxAddForm.get("tax_name")?.setValue(null);
          this.TaxAddForm.get("tax_percentage")?.setValue(null);
          this.TaxAddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        this.NgxSpinnerService.hide();
        // this.ToastrService.success('Opening Balance Added Successfully')
      });
    }
    else { }
  }

  Edit_Tax(parameter: any) {
    this.parameterValue6 = parameter
    console.log(this.parameterValue6, 'this.parameterValue6');
    this.EditTaxForm.get("tax_gid")?.setValue(this.parameterValue6.tax_gid);
    this.EditTaxForm.get("edittax_name")?.setValue(this.parameterValue6.tax_name);
    this.EditTaxForm.get("edittax_percentage")?.setValue(this.parameterValue6.percentage);
  }

  Update_Tax() {
    this.EditTaxForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetupdateTaxDtls';
    this.service.post(url, this.EditTaxForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.EditTaxForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.EditTaxForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  ExternalGLCode_Tax(parameter: any) {
    this.parameterValue7 = parameter
    console.log(this.parameterValue7, 'this.parameterValue7');
    this.TaxExternalGLCodeEdit.get("tax_gid")?.setValue(this.parameterValue7.tax_gid);
    this.externalassetacct_name = this.parameterValue7.tax_name;
    this.TaxExternalGLCodeEdit.get("externalgl_code")?.setValue(this.parameterValue7.external_gl_code);
    this.taxgl_code = this.parameterValue7.gl_code;
  }

  update_Taxexterglcode() {
    this.TaxExternalGLCodeEdit.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetupdateTaxGLCode';
    this.service.post(url, this.TaxExternalGLCodeEdit.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.TaxExternalGLCodeEdit.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.TaxExternalGLCodeEdit.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  ExternalGLCode_Asset(parameter: any) {
    this.parameterValue8 = parameter
    console.log(this.parameterValue8, 'this.parameterValue8');
    this.AssetExternalGLCodeEdit.get("assetdtl_gid")?.setValue(this.parameterValue8.assetdtl_gid);
    this.assetgl_code = this.parameterValue8.assetgl_code;
    this.asset_name = this.parameterValue8.product_name;
    this.AssetExternalGLCodeEdit.get("assetexternalgl_code")?.setValue(this.parameterValue8.external_gl_code);
  }

  update_Assetexterglcode() {
    this.AssetExternalGLCodeEdit.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/GetupdateAssetGLCode';
    this.service.post(url, this.AssetExternalGLCodeEdit.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.AssetExternalGLCodeEdit.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.AssetExternalGLCodeEdit.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  Department_Mapping(parameter: any) {
    this.parameterValue9 = parameter
    this.DepartmentMapForm.get("department_gid")?.setValue(this.parameterValue9.department_gid);
    this.DepartmentMapForm.get("Dept_mapping_account")?.setValue(this.parameterValue9.account_gid);
    this.department_prefix = this.parameterValue9.department_prefix;
    this.department_code = this.parameterValue9.department_code;
    this.department_name = this.parameterValue9.department_name;
  }

  Update_DeptAccount() {
    this.DepartmentMapForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateDepartmentAccountMapping';
    this.service.post(url, this.DepartmentMapForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.DepartmentMapForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.DepartmentMapForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  ExpenseGroup_Mapping(parameter: any) {
    this.parameterValue10 = parameter
    console.log(this.parameterValue10, 'this.parameterValue10');
    this.ExpenseGroupMapForm.get("producttype_gid")?.setValue(this.parameterValue10.producttype_gid);
    this.ExpenseGroupMapForm.get("exaccount_name")?.setValue(this.parameterValue10.account_gid);
    this.expensegroup_code = this.parameterValue10.producttype_code;
    this.expensegroup_name = this.parameterValue10.producttype_name;
  }

  UpdateExpenseGroup_Mapping() {
    this.ExpenseGroupMapForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateExpenseGroupMapping';
    this.service.post(url, this.ExpenseGroupMapForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.ExpenseGroupMapForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.ExpenseGroupMapForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  Submit_AccountMapping() {
    this.AddMapAccountForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/PostAccountMapping';
    this.service.post(url, this.AddMapAccountForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.AddMapAccountForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.AddMapAccountForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  Edit_AcctMapping(parameter: any) {
    this.parameterValue11 = parameter
    console.log(this.parameterValue11, 'this.parameterValue11');
    this.EditMapAccountForm.get("accountmapping_gid")?.setValue(this.parameterValue11.accountmapping_gid);
    this.EditMapAccountForm.get("editmapping_account")?.setValue(this.parameterValue11.account_gid);
    this.editscreen_name = this.parameterValue11.screen;
    this.editfield_name = this.parameterValue11.field;
    this.editmodule_name = this.parameterValue11.module;
  }

  Update_AccountMapping() {
    this.EditMapAccountForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateMapAccount';
    this.service.post(url, this.EditMapAccountForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.EditMapAccountForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.EditMapAccountForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  Debitor_GLCode(parameter: any) {
    this.parameterValue12 = parameter
    console.log(this.parameterValue12, 'this.parameterValue12');
    this.DebtorExternalCodeForm.get("customer_gid")?.setValue(this.parameterValue12.customer_gid);
    this.DebtorExternalCodeForm.get("debtorexternalgl_code")?.setValue(this.parameterValue12.external_gl_code);
    this.debtoracct_code = this.parameterValue12.customer_code;
    this.debtoracct_name = this.parameterValue12.customer_name;
    this.debtorgl_code = this.parameterValue12.gl_code;
    this.DebtorExternalCodeForm.get("debtorrgl_code")?.setValue(this.parameterValue12.gl_code);
  }

  Update_DebtorExternalCode() {
    this.DebtorExternalCodeForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateDebtorExternalCode';
    this.service.post(url, this.DebtorExternalCodeForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.DebtorExternalCodeForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.DebtorExternalCodeForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  Creditor_GLCode(parameter: any) {
    this.parameterValue13 = parameter
    console.log(this.parameterValue13, 'this.parameterValue13');
    this.CreditorExternalCodeForm.get("vendor_gid")?.setValue(this.parameterValue13.vendor_gid);
    this.CreditorExternalCodeForm.get("creditorexternalgl_code")?.setValue(this.parameterValue13.external_gl_code);
    this.creditorvendor_code = this.parameterValue13.vendor_code;
    this.creditorcompany_name = this.parameterValue13.vendor_companyname;
    this.creditorgl_code = this.parameterValue13.gl_code;
    this.CreditorExternalCodeForm.get("creditorgl_code")?.setValue(this.parameterValue13.gl_code);
  }

  Update_CreditorExternalCode() {
    this.CreditorExternalCodeForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateCreditorExternalCode';
    this.service.post(url, this.CreditorExternalCodeForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.CreditorExternalCodeForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      else {
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.success(result.message)
        this.CreditorExternalCodeForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
      }
      this.NgxSpinnerService.hide();
      // this.ToastrService.success('Opening Balance Added Successfully')
    });
  }

  mappingaccountlist = [
    { account_name: 'Sales A/C' },
    { account_name: 'Other Income' },
    { account_name: 'Finance Charges Income' },
    { account_name: 'Sales Return and allowance' },
    { account_name: 'Purchase Discounts' },
    { account_name: 'Round Off Charges' },
  ]

  modulenamelist = [
    { module_name: 'Purchase' },
    { module_name: 'Receivables' },
    { module_name: 'Travel and Expense' }
  ]

  screennamelist = [
    { screen_name: 'Invoice' },
    { screen_name: 'Payment' },
    { screen_name: 'Receipt' }
  ]

  fieldnamelist = [
    { field_name: 'COGS' },
    { field_name: 'Addon Amount' },
    { field_name: 'Additional Discount' },
    { field_name: 'Frieght Charges' },
    { field_name: 'BuyBack/Scrap Value' },
    { field_name: 'With Hold Tax' },
    { field_name: 'Adjustment Amount' },
    { field_name: 'Exchange' },
    { field_name: 'Bank Charges' }
  ]

  toggleOptions(data: any) {
    if (this.showOptionsDivId === data) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = data;
    }
  }

  // Tax Segment Tab Functionality
  Submit_Taxsegment() {
    const params = {
      taxsegment_name: this.taxsegmentaddForm.value.taxsegment_name,
      taxsegment_description: this.taxsegmentaddForm.value.taxsegment_description,
      taxsegment_code: this.taxsegmentaddForm.value.taxsegment_code,
      active_flag: this.taxsegmentaddForm.value.active_flag
    }

    this.taxsegmentaddForm.value;

    if (this.taxsegmentaddForm.status == 'VALID') {
      this.NgxSpinnerService.show();
      var url = 'SmrMstTaxSegment/PostTaxSegment'
      this.service.post(url, params).subscribe((result: any) => {

        if (result.status == false) {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
          this.taxsegmentaddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        else {
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.taxsegmentaddForm.reset();
          this.AccTrnDebtorSummary();
          this.AccTrnCreditorSummary();
          this.AccTrnTaxPayableSummary();
          this.AccTrnAssetDtlsSummary();
          this.AccTrnSalesTypeSummary();
          this.AccTrnPurchaseTypeSummary();
          this.AccTrnEmployeeSummary();
          this.AccTrnAcctMappingSummary();
          this.AccTrnExpenseGroupSummary();
        }
        this.NgxSpinnerService.hide();
        this.TaxSegmentSummary();
        this.taxsegmentaddForm.reset();
      });
    }
    else { }
  }

  TaxSegmentSummary() {
    this.NgxSpinnerService.show();
    var api = 'SmrMstTaxSegment/GetTaxSegmentSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#taxsegmentsummary_list').DataTable().destroy();
      this.responsedata = result;
      this.taxsegmentsummary_list = this.responsedata.TaxSegmentSummary_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#taxsegment_list').DataTable();
      }, 1);
    });
  }

  View_Taxsegment(parameter: any) {
    this.parameterValue14 = parameter
    this.taxsegment_gid = this.parameterValue14.taxsegment_gid;    
    this.viewtaxsegment_name = this.parameterValue14.taxsegment_name;
    this.viewreference_type = this.parameterValue14.reference_type;
  }

  TaxSegment_Mapping(parameter: any) {
    this.parameterValue15 = parameter
    this.taxsegmentMappingForm.get("taxsegment_gid")?.setValue(this.parameterValue15.taxsegment_gid);
    this.taxsegmentMappingForm.get("taxaccount_name")?.setValue(this.parameterValue15.account_gid);
    this.mappingtaxsegment_name = this.parameterValue15.taxsegment_name;
    this.mappingtaxsegment_reftype = this.parameterValue15.reference_type;
  }

  Update_TaxSegmentMapping() {
    this.taxsegmentMappingForm.value;
    this.NgxSpinnerService.show();
    var url = 'GLCode/UpdateTaxSegmentMapping';
    this.service.post(url, this.taxsegmentMappingForm.value).subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
        this.taxsegmentMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
        this.taxsegmentmapping();
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.taxsegmentMappingForm.reset();
        this.AccTrnDebtorSummary();
        this.AccTrnCreditorSummary();
        this.AccTrnTaxPayableSummary();
        this.AccTrnAssetDtlsSummary();
        this.AccTrnSalesTypeSummary();
        this.AccTrnPurchaseTypeSummary();
        this.AccTrnEmployeeSummary();
        this.AccTrnAcctMappingSummary();
        this.AccTrnExpenseGroupSummary();
        this.taxsegmentmapping();

      }
      this.NgxSpinnerService.hide();
    });
  }
TaxAccountMapping(parameter: any){
  this.parameterValue1 = parameter
  this.taxpercentage = this.parameterValue1.percentage
  this.taxname = this.parameterValue1.tax_name
  this.tax_gid = this.parameterValue1.tax_gid

}
OnUpdateTaxMapping(){
  this.NgxSpinnerService.show();
  let param = {
    tax_gid : this.tax_gid,
    account_gid : this.TaxAccountMapForm.value.account_gid
  }
  var url = 'GLCode/UpdateTaxAccountMapping'
  this.service.post(url,param).subscribe((result: any) => {
    if(result.status == true){
      this.ToastrService.success(result.message);
      this.TaxAccountMapForm.reset();
      this.AccTrnDebtorSummary();
      this.AccTrnCreditorSummary();
      this.AccTrnTaxPayableSummary();
      this.AccTrnAssetDtlsSummary();
      this.AccTrnSalesTypeSummary();
      this.AccTrnPurchaseTypeSummary();
      this.AccTrnEmployeeSummary();
      this.AccTrnAcctMappingSummary();
      this.AccTrnExpenseGroupSummary();
      this.taxsegmentmapping();

    }
    else{
      this.ToastrService.warning(result.message);
      this.TaxAccountMapForm.reset();
      this.AccTrnDebtorSummary();
      this.AccTrnCreditorSummary();
      this.AccTrnTaxPayableSummary();
      this.AccTrnAssetDtlsSummary();
      this.AccTrnSalesTypeSummary();
      this.AccTrnPurchaseTypeSummary();
      this.AccTrnEmployeeSummary();
      this.AccTrnAcctMappingSummary();
      this.AccTrnExpenseGroupSummary();
      this.taxsegmentmapping();

    }
    this.NgxSpinnerService.hide();
  });
}
SalaryComponentMapping(parameter: any){
  this.parameterValue1 = parameter
  this.salarycomponent_gid1 = this.parameterValue1.salarycomponent_gid
  this.componentgroup_name = this.parameterValue1.componentgroup_name
  this.component_code = this.parameterValue1.component_code
  this.component_name = this.parameterValue1.component_name
}
OnUpdateSalaryCompMapping(){
  this.NgxSpinnerService.show();
  let param ={
  salarycomponent_gid1 : this.salarycomponent_gid1,
  salarycomponent_ledger_gid : this.SalaryCompMappingForm.value.salarycomponent_ledger_gid
}
var url = 'GLCode/UpdateSalaryComponentMapping'
this.service.post(url,param).subscribe((result: any) => {
  if(result.status == true){
    this.ToastrService.success(result.message);
    this.SalaryCompMappingForm.reset();
    this.AccTrnDebtorSummary();
    this.AccTrnCreditorSummary();
    this.AccTrnTaxPayableSummary();
    this.AccTrnAssetDtlsSummary();
    this.AccTrnSalesTypeSummary();
    this.AccTrnPurchaseTypeSummary();
    this.AccTrnEmployeeSummary();
    this.AccTrnAcctMappingSummary();
    this.AccTrnExpenseGroupSummary();
    this.SalaryComponentSummary();

  }
  else{
    this.ToastrService.warning(result.message);
    this.SalaryCompMappingForm.reset();
    this.AccTrnDebtorSummary();
    this.AccTrnCreditorSummary();
    this.AccTrnTaxPayableSummary();
    this.AccTrnAssetDtlsSummary();
    this.AccTrnSalesTypeSummary();
    this.AccTrnPurchaseTypeSummary();
    this.AccTrnEmployeeSummary();
    this.AccTrnAcctMappingSummary();
    this.AccTrnExpenseGroupSummary();
    this.SalaryComponentSummary();

  }
  this.NgxSpinnerService.hide();
});
}
onclose(){
  this.DepartmentMapForm.reset();
  this.SalaryCompMappingForm.reset();
}
  // Edit_Taxsegment(parameter: any) {
  //   this.parameterValue15 = parameter
  //   console.log(this.parameterValue15, 'this.parameterValue15');
  //   this.taxsegmentEditForm.get("taxsegment_gid")?.setValue(this.parameterValue15.taxsegment_gid);
  //   this.taxsegmentEditForm.get("edittaxsegment_name")?.setValue(this.parameterValue15.taxsegment_name_edit);
  //   // this.salestype_codeview = this.parameterValue15.salestype_code;
  // }

  // Update_Taxsegment() {
  //   this.taxsegmentEditForm.value;
  //   this.NgxSpinnerService.show();
  //   if (this.taxsegmentEditForm.value.taxsegment_name_edit != null) {
  //     for (const control of Object.keys(this.taxsegmentEditForm.controls)) {
  //       this.taxsegmentEditForm.controls[control].markAsTouched();
  //     }
  //     this.taxsegmentEditForm.value;
  //     this.NgxSpinnerService.show();
  //     var url = 'SmrMstTaxSegment/UpdatedTaxSegmentSummary'
  //     this.service.post(url, this.taxsegmentEditForm.value).pipe().subscribe((result: any) => {
  //       this.responsedata = result;
  //       if (result.status == false) {
  //         window.scrollTo({
  //           top: 0, // Code is used for scroll top after event done
  //         });
  //         this.ToastrService.warning(result.message)
  //         this.TaxSegmentSummary();
  //         this.taxsegmentEditForm.reset();
  //       }
  //       else {
  //         this.ToastrService.success(result.message)
  //         this.TaxSegmentSummary();
  //         this.taxsegmentEditForm.reset();
  //       }
  //       this.NgxSpinnerService.show();
  //       this.TaxSegmentSummary();

  //     });
  //   }
  //   else {
  //     window.scrollTo({
  //       top: 0, // Code is used for scroll top after event done
  //     });
  //     this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
  //   }
  // }
}