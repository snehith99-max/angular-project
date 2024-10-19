import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

interface IDocument {
  finyear_gid: string | Blob;
  finyear_range: string;
  document_type: string;
  document_title: string;
  remarks: string;
  fName: string;
}

@Component({
  selector: 'app-pay-mst-employee-form16details',
  templateUrl: './pay-mst-employee-form16details.component.html',
  styleUrls: ['./pay-mst-employee-form16details.component.scss']
})

export class PayMstEmployeeForm16detailsComponent {

  parametervalue: any;
  selectedDate: string | undefined;
  finyear_range: any;
  document!: IDocument;
  filename!: string;

  formDataObject: FormData = new FormData();

  taxmasterForm!: FormGroup;
  personalinfoForm!: FormGroup;
  incometaxForm!: FormGroup;
  quartersinfoForm!: FormGroup;
  incomeForm!: FormGroup;
  tdsinfoForm!: FormGroup;
  anx1Form!: FormGroup;
  anx2Form!: FormGroup;

  AutoIDkey: any;

  file!: File;
  file1!: FileList;
  file_name: any;

  image_path: any;
  allattchement: any[] = [];

  employee_gid: any;
  finyear_gid: any;
  responsedata: any;
  personalinfo: any;
  quartersinfo: any;
  finyear_list: any;
  incometax: any;
  incomeinfo: any;
  mdlFinyear: any;
  mdlDocType: any;

  grosssalary_amount: number = 0;
  perquisites_amount: number = 0;
  profitof_salary: number = 0;
  totalamount: number = 0;

  component1: number = 0;
  component2: number = 0;
  component3: number = 0;
  pfamount1: number = 0;
  pfamount2: number = 0;
  pfamount3: number = 0;
  lessallowancetotal: number = 0;
  balanceamount: number = 0;

  entertainment_allowance: number = 0;
  taxon_emp: number = 0;
  aggreagateofab: number = 0;
  incomecharge_headsalaries: number = 0;
  employeeincome_rs1: number = 0;
  employeeincome_rs2: number = 0;
  employeeincome_rs3: number = 0;
  employeeincome_total: number = 0;
  grosstotal_income: number = 0;

  section80C_i_value: number = 0;
  section80C_ii_value: number = 0;
  section80C_iii_value: number = 0;
  section80C_iv_value: number = 0;
  section80C_v_value: number = 0;
  section80C_vi_value: number = 0;
  section80C_vii_value: number = 0;
  section80C_vii_gross_total: number = 0;
  section80CCC_gross_total: number = 0;
  section80CCD_gross_total: number = 0;
  aggregate3sec_gross_total: number = 0;
  section_overall_gross_total: number = 0;
  section_overall_deductable_total: number = 0;
  section80C_vii_deductable_total: number = 0;
  section80CCC_deductable_total: number = 0;
  section80CCD_deductable_total: number = 0;
  aggregate3sec_deductable_total: number = 0;
  section80CCD1B_deductable_total: number = 0;
  other_section1_deductable: number = 0;
  other_section2_deductable: number = 0;
  other_section3_deductable: number = 0;
  other_section4_deductable: number = 0;
  other_section5_deductable: number = 0;
  aggregate4Asec_deductible_total: number = 0;
  tds_total_income: number = 0;
  total_taxable_income: number = 0;
  educationcess: number = 0;
  tax_on_total_income: number = 0;
  tax_payable: number = 0;
  less_relief: number = 0;
  net_tax_payable: number = 0;
  less_tax_deducted_at_source: number = 0;
  balance_tax_pay_refund: number = 0;
  tdsinfo: any;

  total_tax_deposited_anx1: number = 0;
  totaltax_dep1_anx1: number = 0;
  totaltax_dep3_anx1: number = 0;
  totaltax_dep2_anx1: number = 0;
  totaltax_dep4_anx1: number = 0;
  totaltax_dep5_anx1: number = 0;
  totaltax_dep6_anx1: number = 0;
  totaltax_dep7_anx1: number = 0;
  totaltax_dep8_anx1: number = 0;
  totaltax_dep9_anx1: number = 0;
  totaltax_dep10_anx1: number = 0;
  totaltax_dep11_anx1: number = 0;
  totaltax_dep12_anx1: number = 0;

  totaltax_dep1_anx2: number = 0;
  totaltax_dep2_anx2: number = 0;
  totaltax_dep3_anx2: number = 0;
  totaltax_dep4_anx2: number = 0;
  totaltax_dep5_anx2: number = 0;
  totaltax_dep6_anx2: number = 0;
  totaltax_dep7_anx2: number = 0;
  totaltax_dep8_anx2: number = 0;
  totaltax_dep9_anx2: number = 0;
  totaltax_dep10_anx2: number = 0;
  totaltax_dep11_anx2: number = 0;
  totaltax_dep12_anx2: number = 0;
  total_tax_deposited_anx2: number = 0;

  anx1info: any;
  anx2info: any;
  incometaxinfo: any;

  mdloldtaxslabs: any;
  mdlindividuals: any;
  mdlresident: any;
  mdlresidentsuper: any;
  mdlnewtaxslabs: any;
  mdlincometaxrates: any;

  tax_regime: any;
  assessment_gid: any;
  employeeoldtaxslabsummarylist: any;
  response_data: any;
  employeenewtaxslabsummarylist: any;

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {

    this.personalinfoForm = new FormGroup({
      assessment_gid: new FormControl(''),
      first_name: new FormControl('', [Validators.required]),
      last_name: new FormControl(''),
      active_flag: new FormControl(''),
      dob: new FormControl(''),
      phone: new FormControl(''),
      email_id: new FormControl(''),
      blood_group: new FormControl(''),
      pan_number: new FormControl(''),
      uan_number: new FormControl(''),
      address_line1: new FormControl(''),
      address_line2: new FormControl(''),
      state: new FormControl(''),
      country: new FormControl(''),
      city: new FormControl(''),
      postal_code: new FormControl(''),
    });

    this.taxmasterForm = new FormGroup({
      tax_regime: new FormControl(''),
      old_tax_slabs: new FormControl(''),
      individuals: new FormControl(''),
      resident: new FormControl(''),
      residentsuper: new FormControl(''),
      new_tax_slabs: new FormControl(''),
      income_tax_rates: new FormControl(''),
    });

    this.incometaxForm = new FormGroup({
      finyear_start: new FormControl(''),
      finyear_gid: new FormControl(''),
      document_type: new FormControl('', [Validators.required]),
      finyear_range: new FormControl(''),
      document_title: new FormControl('', [Validators.required]),
      remarks: new FormControl(''),
      fName: new FormControl('')
    })

    this.incomeForm = new FormGroup({
      grosssalary_amount: new FormControl(''),
      perquisites_amount: new FormControl(''),
      profitof_salary: new FormControl(''),
      totalamount: new FormControl(''),
      component1: new FormControl(''),
      pfamount1: new FormControl(''),
      component2: new FormControl(''),
      pfamount2: new FormControl(''),
      component3: new FormControl(''),
      pfamount3: new FormControl(''),
      balanceamount: new FormControl(''),
      entertainment_allowance: new FormControl(''),
      taxon_emp: new FormControl(''),
      aggreagateofab: new FormControl(''),
      incomecharge_headsalaries: new FormControl(''),
      employee_income1: new FormControl(''),
      employeeincome_rs1: new FormControl(''),
      employee_income2: new FormControl(''),
      employeeincome_rs2: new FormControl(''),
      employee_income3: new FormControl(''),
      employeeincome_rs3: new FormControl(''),
      employeeincome_total: new FormControl(''),
      grosstotal_income: new FormControl(''),
      assessment_gid: new FormControl(''),
      employee_gid: new FormControl(''),
      lessallowancetotal: new FormControl(''),
    })

    this.tdsinfoForm = new FormGroup({
      employee_gid: new FormControl(''),
      assessment_gid: new FormControl(''),
      tds_gid: new FormControl(''),
      grosstotal_income: new FormControl(''),
      section80C_i_name: new FormControl(''),
      section80C_i_value: new FormControl(''),
      section80C_ii_name: new FormControl(''),
      section80C_ii_value: new FormControl(''),
      section80C_iii_name: new FormControl(''),
      section80C_iii_value: new FormControl(''),
      section80C_iv_name: new FormControl(''),
      section80C_iv_value: new FormControl(''),
      section80C_v_name: new FormControl(''),
      section80C_v_value: new FormControl(''),
      section80C_vi_name: new FormControl(''),
      section80C_vi_value: new FormControl(''),
      section80C_vii_name: new FormControl(''),
      section80C_vii_value: new FormControl(''),
      section80C_vii_gross_total: new FormControl(''),
      section80C_vii_deductable_total: new FormControl(''),
      section80CCC_gross_total: new FormControl(''),
      section80CCC_deductable_total: new FormControl(''),
      section80CCD_gross_total: new FormControl(''),
      section80CCD_deductable_total: new FormControl(''),
      aggregate3sec_gross_total: new FormControl(''),
      aggregate3sec_deductable_total: new FormControl(''),
      section80CCD1B_gross_total: new FormControl(''),
      section80CCD1B_deductable_total: new FormControl(''),
      section_overall_gross_total: new FormControl(''),
      section_overall_deductable_total: new FormControl(''),
      other_section1_value: new FormControl(''),
      other_section1_gross_amount: new FormControl(''),
      other_section1_qualifying_amount: new FormControl(''),
      other_section1_deductable: new FormControl(''),
      other_section2_value: new FormControl(''),
      other_section2_gross_amount: new FormControl(''),
      other_section2_qualifying_amount: new FormControl(''),
      other_section2_deductable: new FormControl(''),
      other_section3_value: new FormControl(''),
      other_section3_gross_amount: new FormControl(''),
      other_section3_qualifying_amount: new FormControl(''),
      other_section3_deductable: new FormControl(''),
      other_section4_value: new FormControl(''),
      other_section4_gross_amount: new FormControl(''),
      other_section4_qualifying_amount: new FormControl(''),
      other_section4_deductable: new FormControl(''),
      other_section5_value: new FormControl(''),
      other_section5_gross_amount: new FormControl(''),
      other_section5_qualifying_amount: new FormControl(''),
      other_section5_deductable: new FormControl(''),
      aggregate4Asec_deductible_total: new FormControl(''),
      total_taxable_income: new FormControl(''),
      tax_on_total_income: new FormControl(''),
      educationcess: new FormControl(''),
      tax_payable: new FormControl(''),
      less_relief: new FormControl(''),
      net_tax_payable: new FormControl(''),
      less_tax_deducted_at_source: new FormControl(''),
      balance_tax_pay_refund: new FormControl(''),
    })
  }

  ngOnInit() {
    const assessment_gid = this.route.snapshot.paramMap.get('assessment_gid');
    this.assessment_gid = assessment_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.assessment_gid, secretKey).toString(enc.Utf8);

    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    var api1 = 'PayMstEmployeeAssessmentSummary/GetFinYearDropdown';
    this.service.get(api1).subscribe((result: any) => {
      this.finyear_list = result.getfinyeardropdownlist;
    });

    this.GetEmployeePersonalData();
    this.GetEmployeeOldTaxSlabSummary();
    this.GetEmployeeNewTaxSlabSummary();
    this.GetEmployeeIncomeTaxSummary();
    this.GetEmployeeIncomedata();
    this.GetEmployeeTDSData();
  }

  formatDate(date: string): string {
    if (!date) {
      return '';
    }
    // Assuming date is a string like "2024-03-25"
    const parts = date.split('-');
    return `${parts[2]}-${parts[1]}-${parts[0]}`;
  }

  get document_type() {
    return this.incometaxForm.get('document_type')!;
  }

  get document_title() {
    return this.incometaxForm.get('document_title')!;
  }

  GetEmployeePersonalData() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeePersonalData';

    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.personalinfo = result;

      const dob = this.personalinfo.employee_dob;
      const formattedDob = this.formatDate(dob);

      this.personalinfoForm.get("first_name")?.setValue(this.personalinfo.user_firstname);
      this.personalinfoForm.get("last_name")?.setValue(this.personalinfo.user_lastname);
      this.personalinfoForm.get("dob")?.setValue(formattedDob);
      this.personalinfoForm.get("email_id")?.setValue(this.personalinfo.employee_emailid);
      this.personalinfoForm.get("active_flag")?.setValue(this.personalinfo.employee_gender);
      this.personalinfoForm.get("pan_number")?.setValue(this.personalinfo.pan_no);
      this.personalinfoForm.get("uan_number")?.setValue(this.personalinfo.uan_no);
      this.personalinfoForm.get("blood_group")?.setValue(this.personalinfo.bloodgroup);
      this.personalinfoForm.get("phone")?.setValue(this.personalinfo.employee_mobileno);
      this.personalinfoForm.get("address_line1")?.setValue(this.personalinfo.address1);
      this.personalinfoForm.get("address_line2")?.setValue(this.personalinfo.address2);
      this.personalinfoForm.get("city")?.setValue(this.personalinfo.city);
      this.personalinfoForm.get("state")?.setValue(this.personalinfo.state);
      this.personalinfoForm.get("postal_code")?.setValue(this.personalinfo.postal_code);
      this.personalinfoForm.get("country")?.setValue(this.personalinfo.country_name);
    });
  }

  GetEmployeeOldTaxSlabSummary() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeOldTaxSlabSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.employeeoldtaxslabsummarylist = this.response_data.employeeoldtaxslabsummary_list;
      setTimeout(() => {
        $('#income_tax_old_regime_list').DataTable();
      }, 1);
    });
  }

  GetEmployeeNewTaxSlabSummary() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeNewTaxSlabSummary';
    this.service.get(api).subscribe((result: any) => {
      this.response_data = result;
      this.employeenewtaxslabsummarylist = this.response_data.employeenewtaxslabsummary_list;
      setTimeout(() => {
        $('#income_tax_old_regime_list').DataTable();
      }, 1);
    });
  }

  incometaxdocumentsubmit() {
    const api = 'PayMstEmployeeAssessmentSummary/PostEmployeeIncomeTaxDocument'
    const allattchement = "" + JSON.stringify(this.allattchement) + "";
    console.log(this.incometaxForm.value)

    this.document = this.incometaxForm.value;
    if (this.file1 != null && this.file1 != undefined) {

      this.formDataObject.append("filename", allattchement);
      this.formDataObject.append("finyear_range", this.document.finyear_range);
      this.formDataObject.append("document_type", this.document.document_type);
      this.formDataObject.append("document_title", this.document.document_title);
      this.formDataObject.append("remarks", this.document.remarks);
      this.formDataObject.append("fName", this.document.fName);

      this.service.post(api, this.formDataObject).subscribe((result: any) => {
        this.responsedata = result;
        if (result.status == false) {
          this.ToastrService.warning(result.message)
        }
        else {
          this.ToastrService.success(result.message)
        }
        setTimeout(function () {
          window.location.reload();
        }, 2000); // 2000 milliseconds = 2 seconds
      });
    }
  }

  downloadfile(document_upload: string, document_title: string) {
    const image = document_upload.split('.net/');
    const page = image[0];
    const url = page.split('?');
    const imageurl = url[0];
    const parts = imageurl.split('.');
    const extension = parts.pop();

    this.service.downloadfile(imageurl, document_title + '.' + extension).subscribe(
      (data: any) => {
        if (data != null) {
          this.service.filedownload1(data);
        } else {
          this.ToastrService.warning('Error in file download');
        }
      },
    );
  }

  onChange2($event: any): void {
    this.file1 = $event.target.files;

    if (this.file1 != null && this.file1.length !== 0) {
      for (let i = 0; i < this.file1.length; i++) {
        this.AutoIDkey = this.generateKey();
        this.formDataObject.append(this.AutoIDkey, this.file1[i]);
        this.file_name = this.file1[i].name;
        this.allattchement.push({
          AutoID_Key: this.AutoIDkey,
          file_name: this.file1[i].name
        });
        console.log(this.file1[i]);
      }
    }
  }

  generateKey(): string {
    return `AutoIDKey${new Date().getTime()}`;
  }

  GetEmployeeIncomeTaxSummary() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeIncomeTaxSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#incometax').DataTable().destroy();
      this.incometax = result.employeeincometaxsummary_lists;
      setTimeout(() => {
        $('#incometax').DataTable();
      }, 1);
    });
  }

  GetEmployeeIncomedata() {
    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeIncomedata';

    const assessment_gid = this.route.snapshot.paramMap.get('assessment_gid');
    this.assessment_gid = assessment_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.assessment_gid, secretKey).toString(enc.Utf8);

    let param = {
      assessment_gid: deencryptedParam
    }

    this.service.getparams(api, param).subscribe((result: any) => {

      this.responsedata = result;
      this.incomeinfo = result;

      this.incomeForm.get("employee_gid")?.setValue(this.incomeinfo.employee_gid);
      this.incomeForm.get("assessment_gid")?.setValue(this.incomeinfo.assessment_gid);
      this.incomeForm.get("finyear_gid")?.setValue(this.incomeinfo.finyear_gid);
      this.incomeForm.get("grosssalary_amount")?.setValue(this.incomeinfo.grosssalary_amount);
      this.incomeForm.get("perquisites_amount")?.setValue(this.incomeinfo.perquisites_amount);
      this.incomeForm.get("profitof_salary")?.setValue(this.incomeinfo.profitof_salary);
      this.incomeForm.get("totalamount")?.setValue(this.incomeinfo.totalamount);
      this.incomeForm.get("component1")?.setValue(this.incomeinfo.component1);
      this.incomeForm.get("component2")?.setValue(this.incomeinfo.component2);
      this.incomeForm.get("component3")?.setValue(this.incomeinfo.component3);
      this.incomeForm.get("pfamount1")?.setValue(this.incomeinfo.pfamount1);
      this.incomeForm.get("pfamount2")?.setValue(this.incomeinfo.pfamount2);
      this.incomeForm.get("pfamount3")?.setValue(this.incomeinfo.pfamount3);
      this.incomeForm.get("lessallowancetotal")?.setValue(this.incomeinfo.lessallowancetotal);
      this.incomeForm.get("balanceamount")?.setValue(this.incomeinfo.balanceamount);
      this.incomeForm.get("entertainment_allowance")?.setValue(this.incomeinfo.entertainment_allowance);
      this.incomeForm.get("taxon_emp")?.setValue(this.incomeinfo.taxon_emp);
      this.incomeForm.get("aggreagateofab")?.setValue(this.incomeinfo.aggreagateofab);
      this.incomeForm.get("incomecharge_headsalaries")?.setValue(this.incomeinfo.incomecharge_headsalaries);
      this.incomeForm.get("employee_income1")?.setValue(this.incomeinfo.employee_income1);
      this.incomeForm.get("employee_income2")?.setValue(this.incomeinfo.employee_income2);
      this.incomeForm.get("employee_income3")?.setValue(this.incomeinfo.employee_income3);
      this.incomeForm.get("employeeincome_rs1")?.setValue(this.incomeinfo.employeeincome_rs1);
      this.incomeForm.get("employeeincome_rs2")?.setValue(this.incomeinfo.employeeincome_rs2);
      this.incomeForm.get("employeeincome_rs3")?.setValue(this.incomeinfo.employeeincome_rs3);
      this.incomeForm.get("employeeincome_total")?.setValue(this.incomeinfo.employeeincome_total);
      this.incomeForm.get("grosstotal_income")?.setValue(this.incomeinfo.grosstotal_income);
    });
  }

  submitincomedetails() {
    const assessment_gid = this.route.snapshot.paramMap.get('assessment_gid');
    this.assessment_gid = assessment_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.assessment_gid, secretKey).toString(enc.Utf8);
    var api = 'PayMstEmployeeAssessmentSummary/PostEmployeeIncomeData';
    this.incomeForm.get("assessment_gid")?.setValue(deencryptedParam);

    this.service.post(api, this.incomeForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  GetEmployeeTDSData() {
    const assessment_gid = this.route.snapshot.paramMap.get('assessment_gid');
    this.assessment_gid = assessment_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.assessment_gid, secretKey).toString(enc.Utf8);

    var api = 'PayMstEmployeeAssessmentSummary/GetEmployeeTDSData';

    let param = {
      assessment_gid: deencryptedParam,
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.tdsinfo = result;

      this.tdsinfoForm.get("assessment_gid")?.setValue(this.tdsinfo.assessment_gid);
      this.tdsinfoForm.get("section80C_i_name")?.setValue(this.tdsinfo.section80C_i_name);
      this.tdsinfoForm.get("section80C_i_value")?.setValue(this.tdsinfo.section80C_i_value);
      this.tdsinfoForm.get("section80C_ii_name")?.setValue(this.tdsinfo.section80C_ii_name);
      this.tdsinfoForm.get("section80C_ii_value")?.setValue(this.tdsinfo.section80C_ii_value);
      this.tdsinfoForm.get("section80C_iii_name")?.setValue(this.tdsinfo.section80C_iii_name);
      this.tdsinfoForm.get("section80C_iii_value")?.setValue(this.tdsinfo.section80C_iii_value);
      this.tdsinfoForm.get("section80C_iv_name")?.setValue(this.tdsinfo.section80C_iv_name);
      this.tdsinfoForm.get("section80C_iv_value")?.setValue(this.tdsinfo.section80C_iv_value);
      this.tdsinfoForm.get("section80C_v_name")?.setValue(this.tdsinfo.section80C_v_name);
      this.tdsinfoForm.get("section80C_v_value")?.setValue(this.tdsinfo.section80C_v_value);
      this.tdsinfoForm.get("section80C_vi_name")?.setValue(this.tdsinfo.section80C_vi_name);
      this.tdsinfoForm.get("section80C_vi_value")?.setValue(this.tdsinfo.section80C_vi_value);
      this.tdsinfoForm.get("section80C_vii_name")?.setValue(this.tdsinfo.section80C_vii_name);
      this.tdsinfoForm.get("section80C_vii_value")?.setValue(this.tdsinfo.section80C_vii_value);
      this.tdsinfoForm.get("section80C_vii_gross_total")?.setValue(this.tdsinfo.section80C_vii_gross_total);
      this.tdsinfoForm.get("section80C_vii_deductable_total")?.setValue(this.tdsinfo.section80C_vii_deductable_total);
      this.tdsinfoForm.get("section80CCC_gross_total")?.setValue(this.tdsinfo.section80CCC_gross_total);
      this.tdsinfoForm.get("section80CCC_deductable_total")?.setValue(this.tdsinfo.section80CCC_deductable_total);
      this.tdsinfoForm.get("section80CCD_gross_total")?.setValue(this.tdsinfo.section80CCD_gross_total);
      this.tdsinfoForm.get("section80CCD_deductable_total")?.setValue(this.tdsinfo.section80CCD_deductable_total);
      this.tdsinfoForm.get("aggregate3sec_gross_total")?.setValue(this.tdsinfo.aggregate3sec_gross_total);
      this.tdsinfoForm.get("aggregate3sec_deductable_total")?.setValue(this.tdsinfo.aggregate3sec_deductable_total);
      this.tdsinfoForm.get("section80CCD1B_gross_total")?.setValue(this.tdsinfo.section80CCD1B_gross_total);
      this.tdsinfoForm.get("section80CCD1B_deductable_total")?.setValue(this.tdsinfo.section80CCD1B_deductable_total);
      this.tdsinfoForm.get("other_section1_value")?.setValue(this.tdsinfo.other_section1_value);
      this.tdsinfoForm.get("other_section1_gross_amount")?.setValue(this.tdsinfo.other_section1_gross_amount);
      this.tdsinfoForm.get("other_section1_qualifying_amount")?.setValue(this.tdsinfo.other_section1_qualifying_amount);
      this.tdsinfoForm.get("other_section1_deductable")?.setValue(this.tdsinfo.other_section1_deductable);
      this.tdsinfoForm.get("other_section2_value")?.setValue(this.tdsinfo.other_section2_value);
      this.tdsinfoForm.get("other_section2_gross_amount")?.setValue(this.tdsinfo.other_section2_gross_amount);
      this.tdsinfoForm.get("other_section2_qualifying_amount")?.setValue(this.tdsinfo.other_section2_qualifying_amount);
      this.tdsinfoForm.get("other_section2_deductable")?.setValue(this.tdsinfo.other_section2_deductable);
      this.tdsinfoForm.get("other_section3_value")?.setValue(this.tdsinfo.other_section3_value);
      this.tdsinfoForm.get("other_section3_gross_amount")?.setValue(this.tdsinfo.other_section3_gross_amount);
      this.tdsinfoForm.get("other_section3_qualifying_amount")?.setValue(this.tdsinfo.other_section3_qualifying_amount);
      this.tdsinfoForm.get("other_section3_deductable")?.setValue(this.tdsinfo.other_section3_deductable);
      this.tdsinfoForm.get("other_section4_value")?.setValue(this.tdsinfo.other_section4_value);
      this.tdsinfoForm.get("other_section4_gross_amount")?.setValue(this.tdsinfo.other_section4_gross_amount);
      this.tdsinfoForm.get("other_section4_qualifying_amount")?.setValue(this.tdsinfo.other_section4_qualifying_amount);
      this.tdsinfoForm.get("other_section4_deductable")?.setValue(this.tdsinfo.other_section4_deductable);
      this.tdsinfoForm.get("other_section5_value")?.setValue(this.tdsinfo.other_section5_value);
      this.tdsinfoForm.get("other_section5_gross_amount")?.setValue(this.tdsinfo.other_section5_gross_amount);
      this.tdsinfoForm.get("other_section5_qualifying_amount")?.setValue(this.tdsinfo.other_section5_qualifying_amount);
      this.tdsinfoForm.get("other_section5_deductable")?.setValue(this.tdsinfo.other_section5_deductable);
      this.tdsinfoForm.get("aggregate4Asec_deductible_total")?.setValue(this.tdsinfo.aggregate4Asec_deductible_total);
      this.tdsinfoForm.get("total_taxable_income")?.setValue(this.tdsinfo.total_taxable_income);
      this.tdsinfoForm.get("tax_on_total_income")?.setValue(this.tdsinfo.tax_on_total_income);
      this.tdsinfoForm.get("educationcess")?.setValue(this.tdsinfo.educationcess);
      this.tdsinfoForm.get("tax_payable")?.setValue(this.tdsinfo.tax_payable);
      this.tdsinfoForm.get("less_relief")?.setValue(this.tdsinfo.less_relief);
      this.tdsinfoForm.get("net_tax_payable")?.setValue(this.tdsinfo.net_tax_payable);
      this.tdsinfoForm.get("less_tax_deducted_at_source")?.setValue(this.tdsinfo.less_tax_deducted_at_source);
      this.tdsinfoForm.get("balance_tax_pay_refund")?.setValue(this.tdsinfo.balance_tax_pay_refund);
      this.tdsinfoForm.get("tds_gid")?.setValue(this.tdsinfo.tds_gid);

    });
  }

  Updatetds() {
    let 
      param = {
        tds_gid : this.tdsinfoForm.value.tds_gid
      } 
    

      var api = 'PayMstEmployeeAssessmentSummary/PostEmployeeTDSData';
    this.service.getparams(api,param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  grosssalarytotal() {
    this.totalamount = (+this.grosssalary_amount || 0) + (+this.perquisites_amount || 0) + (+this.profitof_salary || 0);
    this.balanceamounttotal();
  }

  allowancesalarytotal() {
    this.lessallowancetotal = (+this.pfamount1 || 0) + (+this.pfamount2 || 0) + (+this.pfamount3 || 0);
    this.balanceamounttotal();
  }

  balanceamounttotal() {
    this.balanceamount = this.totalamount - this.lessallowancetotal;
    this.incomechargeabletotal();
  }

  aggreagateof_ab() {
    this.aggreagateofab = ((+this.entertainment_allowance) + (+this.taxon_emp));
    this.incomechargeabletotal();
  }

  incomechargeabletotal() {
    this.incomecharge_headsalaries = this.balanceamount - this.aggreagateofab;
    this.grosstotal();
  }

  otherincometotal() {
    this.employeeincome_total = ((+this.employeeincome_rs1) + (+this.employeeincome_rs2) + (+this.employeeincome_rs3));
    this.grosstotal();
  }

  grosstotal() {
    this.grosstotal_income = this.incomecharge_headsalaries - this.employeeincome_total;
    console.log('Gross Total Income:', this.grosstotal_income);
    this.incomeForm.get('grosstotal_income')?.setValue(this.grosstotal_income);
  }

  section80cgrosscalc() {
    this.section80C_vii_gross_total = ((+this.section80C_i_value) + (+this.section80C_ii_value) + (+this.section80C_iii_value) + (+this.section80C_iv_value) + (+this.section80C_v_value) + (+this.section80C_vi_value) + (+this.section80C_vii_value))
  }

  aggregate3sectionsgrosscalc() {
    this.aggregate3sec_gross_total = (+this.section80C_vii_gross_total) + (+this.section80CCC_gross_total) + (+this.section80CCD_gross_total);
  }

  sectionoverallgrosscalc() {
    this.section_overall_gross_total = (+this.section80C_vii_gross_total) + (+this.section80CCC_gross_total) + (+this.section80CCD_gross_total) + (+this.aggregate3sec_gross_total);
  }

  sectionoveralldeductablecalc() {
    this.section_overall_gross_total = (+this.section80C_vii_deductable_total) + (+this.section80CCC_deductable_total) + (+this.section80CCD_deductable_total) + (+this.aggregate3sec_deductable_total) + (+this.section80CCD1B_deductable_total);
  }

  // aggregate4Asectiondeductiblescalc() {
  //   this.aggregate4Asec_deductible_total = (+this.other_section1_deductable) + (+this.other_section2_deductable) + (+this.other_section3_deductable) + (+this.other_section4_deductable) + (+this.other_section5_deductable);
  //   console.log('Aggregate Deductible Total:', this.aggregate4Asec_deductible_total);
  //   this.totaltaxableincome();
  // }

  aggregate4Asectiondeductiblescalc() {
    this.aggregate4Asec_deductible_total = (+this.section80C_vii_deductable_total) + (+this.section80CCC_deductable_total) + (+this.section80CCD_deductable_total) + (+this.aggregate3sec_deductable_total) + (+this.section80CCD1B_deductable_total) + (+this.other_section1_deductable) + (+this.other_section2_deductable) + (+this.other_section3_deductable) + (+this.other_section4_deductable) + (+this.other_section5_deductable);
    this.totaltaxableincome();
  }

  totaltaxableincome() {
    const grosstotal_income = parseFloat(this.grosstotal_income.toString().replace(/,/g, ''));
    const aggregate4Asec_deductible_total = this.aggregate4Asec_deductible_total || 0;
    this.total_taxable_income = grosstotal_income - aggregate4Asec_deductible_total;
    this.tdsinfoForm.get('total_taxable_income')?.setValue(this.total_taxable_income);
  }

  educationcesscalc() {
    const tax_on_total_income = this.tax_on_total_income;
    const educationcesspercent = 3;
    this.educationcess = (tax_on_total_income * educationcesspercent) / 100;
    this.tax_payable = (+tax_on_total_income) + (+this.educationcess);
  }

  nettaxpayablecalc() {
    const tax_payable_amt = this.tax_payable;
    this.net_tax_payable = (+tax_payable_amt) - (+this.less_relief);
  }

  balancetaxpayrefundcalc() {
    const net_tax_payable_amt = this.net_tax_payable;
    this.balance_tax_pay_refund = (+net_tax_payable_amt) - (+this.less_tax_deducted_at_source);
  }

  totaltaxdepanx1calc() {
    this.total_tax_deposited_anx1 = ((+this.totaltax_dep1_anx1) + (+this.totaltax_dep2_anx1) + (+this.totaltax_dep3_anx1) + (+this.totaltax_dep4_anx1) + (+this.totaltax_dep5_anx1) + (+this.totaltax_dep6_anx1) + (+this.totaltax_dep7_anx1) + (+this.totaltax_dep8_anx1) + (+this.totaltax_dep9_anx1) + (+this.totaltax_dep10_anx1) + (+this.totaltax_dep11_anx1) + (+this.totaltax_dep12_anx1))
  }

  totaltaxdepanx2calc() {
    this.total_tax_deposited_anx2 = ((+this.totaltax_dep1_anx2) + (+this.totaltax_dep2_anx2) + (+this.totaltax_dep3_anx2) + (+this.totaltax_dep4_anx2) + (+this.totaltax_dep5_anx2) + (+this.totaltax_dep6_anx2) + (+this.totaltax_dep7_anx2) + (+this.totaltax_dep8_anx2) + (+this.totaltax_dep9_anx2) + (+this.totaltax_dep10_anx2) + (+this.totaltax_dep11_anx2) + (+this.totaltax_dep12_anx2))
  }

  back() {
    this.router.navigate(['/payroll/PayMstEmpAssessmentsummary'])
  }

  ondelete() {
    this.NgxSpinnerService.show();
    var params = {
      taxdocument_gid: this.parametervalue
    }
    var url = 'PayMstAssessmentSummary/deleteincometaxsummary';
    this.service.getparams(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }
    })
  }

  openModaldelete(taxdocument_gid: any) {
    this.parametervalue = taxdocument_gid
  }

  GetIncometaxdata() {
    var api = 'PayMstAssessmentSummary/GetIncometaxdata';

    let param = {
      employee_gid: this.employee_gid
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      debugger
      this.responsedata = result;
      this.incometaxinfo = result;

      this.incometaxForm.get("finyear_range")?.setValue(this.incometaxinfo.fin_year);
      this.incometaxForm.get("document_type")?.setValue(this.incometaxinfo.documenttype_gid);
      this.incometaxForm.get("document_title")?.setValue(this.incometaxinfo.document_title);
      this.incometaxForm.get("remarks")?.setValue(this.incometaxinfo.remarks);
    });
  }
}


// ondelete(taxdocument_gid: any) {
//   this.parametervalue = taxdocument_gid
// }

// ondelete() {
//   this.NgxSpinnerService.show();
//   var params = {
//     taxdocument_gid: this.parametervalue
//   }
//   var url = 'PayMstEmployeeAssessmentSummary/DeleteEmployeeIncomeTaxSummary';
//   this.service.getparams(url, params).subscribe((result: any) => {
//     if (result.status == true) {
//       this.NgxSpinnerService.hide();
//       this.ToastrService.success(result.message);
//       this.ngOnInit();
//     }
//     else {
//       this.ToastrService.warning(result.messagge);
//       this.NgxSpinnerService.hide();
//     }
//   })
// } 
