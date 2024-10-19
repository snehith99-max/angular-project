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
  finyear_range: string;
  document_type: string;
  document_title: string;
  remarks: string;
  fName: string;
}

@Component({
  selector: 'app-pay-mst-form16employeedetail',
  templateUrl: './pay-mst-form16employeedetail.component.html',
  styleUrls: ['./pay-mst-form16employeedetail.component.scss']
})

export class PayMstForm16employeedetailComponent {
  parametervalue: any;
  selectedDate: string | undefined;
  finyear_range: any;
  document!: IDocument;
  filename!: string;

  formDataObject: FormData = new FormData();

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
  assessment_gid: any;
  responsedata: any;
  personalinfo: any;
  quartersinfo: any;
  finyear_list: any;
  incometax: any;
  incomeinfo: any;
  mdlFinyear: any;
  mdlDocType: any;

  q1_amt_paid_credited: number = 0;
  q2_amt_paid_credited: any;
  q3_amt_paid_credited: any;
  q4_amt_paid_credited: number = 0;
  total_amt_paid_credited: number = 0;

  q1_amt_tax_deducted: number = 0;
  q2_amt_tax_deducted: number = 0;
  q3_amt_tax_deducted: number = 0;
  q4_amt_tax_deducted: number = 0;
  total_amt_tax_deducted: number = 0;

  q1_amt_tax_deposited: number = 0;
  q2_amt_tax_deposited: number = 0;
  q3_amt_tax_deposited: number = 0;
  q4_amt_tax_deposited: number = 0;
  total_amt_tax_deposited: number = 0;

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
  tax_regime: any;
  taxpercentold: any;
  taxpercentnew: any;  

  constructor(private fb: FormBuilder, private ToastrService: ToastrService,
    private route: ActivatedRoute, private router: Router, private service: SocketService,
    public NgxSpinnerService: NgxSpinnerService) {

    this.personalinfoForm = new FormGroup({
      employee_gid: new FormControl(''),
      first_name: new FormControl('', [Validators.required]),
      last_name: new FormControl(''),
      active_flag: new FormControl(''),
      dob: new FormControl(''),
      phone: new FormControl('', [Validators.required, Validators.pattern('[0-9]{10}$'), Validators.minLength(1), Validators.maxLength(10)]),
      email_id: new FormControl('', [Validators.pattern('^([a-z0-9]+(?:[-.][a-z0-9]+)*)@([a-z0-9]+\\.[a-z]{2,20}(\\.[a-z]{2})?|\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\]|localhost)$')]),
      blood_group: new FormControl(''),
      pan_number: new FormControl(''),
      uan_number: new FormControl(''),
      address_line1: new FormControl(''),
      address_line2: new FormControl(''),
      state: new FormControl(''),
      country: new FormControl(''),
      city: new FormControl(''),
      postal_code: new FormControl('', [Validators.pattern('[0-9]{6}$'), Validators.maxLength(6)]),
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

    this.quartersinfoForm = new FormGroup({
      employee_gid: new FormControl(''),
      assessment_gid: new FormControl(''),
      q1_rpt_original_statement: new FormControl(''),
      q1_amt_paid_credited: new FormControl(''),
      q1_amt_tax_deducted: new FormControl(''),
      q1_amt_tax_deposited: new FormControl(''),
      q2_rpt_original_statement: new FormControl(''),
      q2_amt_paid_credited: new FormControl(''),
      q2_amt_tax_deducted: new FormControl(''),
      q2_amt_tax_deposited: new FormControl(''),
      q3_rpt_original_statement: new FormControl(''),
      q3_amt_paid_credited: new FormControl(''),
      q3_amt_tax_deducted: new FormControl(''),
      q3_amt_tax_deposited: new FormControl(''),
      q4_rpt_original_statement: new FormControl(''),
      q4_amt_paid_credited: new FormControl(''),
      q4_amt_tax_deducted: new FormControl(''),
      q4_amt_tax_deposited: new FormControl(''),
      total_amt_paid_credited: new FormControl(''),
      total_amt_tax_deducted: new FormControl(''),
      total_amt_tax_deposited: new FormControl(''),
    });

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
      tax_regime: new FormControl(''),
      taxpercent: new FormControl(''),
      taxpercentold: new FormControl(''),
      taxpercentnew: new FormControl(''),
      tax_on_total_income: new FormControl(''),
      educationcess: new FormControl(''),
      tax_payable: new FormControl(''),
      less_relief: new FormControl(''),
      net_tax_payable: new FormControl(''),
      less_tax_deducted_at_source: new FormControl(''),
      balance_tax_pay_refund: new FormControl(''),
    })

    this.anx1Form = new FormGroup({
      employee_gid: new FormControl(''),
      assessment_gid: new FormControl(''),
      totaltax_dep1_anx1: new FormControl(''),
      receiptnum1_anx1: new FormControl(''),
      ddonum1_anx1: new FormControl(''),
      date1_anx1: new FormControl(''),
      status1_anx1: new FormControl(''),
      totaltax_dep2_anx1: new FormControl(''),
      receiptnum2_anx1: new FormControl(''),
      ddonum2_anx1: new FormControl(''),
      date2_anx1: new FormControl(''),
      status2_anx1: new FormControl(''),
      totaltax_dep3_anx1: new FormControl(''),
      receiptnum3_anx1: new FormControl(''),
      ddonum3_anx1: new FormControl(''),
      date3_anx1: new FormControl(''),
      status3_anx1: new FormControl(''),
      totaltax_dep4_anx1: new FormControl(''),
      receiptnum4_anx1: new FormControl(''),
      ddonum4_anx1: new FormControl(''),
      date4_anx1: new FormControl(''),
      status4_anx1: new FormControl(''),
      totaltax_dep5_anx1: new FormControl(''),
      receiptnum5_anx1: new FormControl(''),
      ddonum5_anx1: new FormControl(''),
      date5_anx1: new FormControl(''),
      status5_anx1: new FormControl(''),
      totaltax_dep6_anx1: new FormControl(''),
      receiptnum6_anx1: new FormControl(''),
      ddonum6_anx1: new FormControl(''),
      date6_anx1: new FormControl(''),
      status6_anx1: new FormControl(''),
      totaltax_dep7_anx1: new FormControl(''),
      receiptnum7_anx1: new FormControl(''),
      ddonum7_anx1: new FormControl(''),
      date7_anx1: new FormControl(''),
      status7_anx1: new FormControl(''),
      totaltax_dep8_anx1: new FormControl(''),
      receiptnum8_anx1: new FormControl(''),
      ddonum8_anx1: new FormControl(''),
      date8_anx1: new FormControl(''),
      status8_anx1: new FormControl(''),
      totaltax_dep9_anx1: new FormControl(''),
      receiptnum9_anx1: new FormControl(''),
      ddonum9_anx1: new FormControl(''),
      date9_anx1: new FormControl(''),
      status9_anx1: new FormControl(''),
      totaltax_dep10_anx1: new FormControl(''),
      receiptnum10_anx1: new FormControl(''),
      ddonum10_anx1: new FormControl(''),
      date10_anx1: new FormControl(''),
      status10_anx1: new FormControl(''),
      totaltax_dep11_anx1: new FormControl(''),
      receiptnum11_anx1: new FormControl(''),
      ddonum11_anx1: new FormControl(''),
      date11_anx1: new FormControl(''),
      status11_anx1: new FormControl(''),
      totaltax_dep12_anx1: new FormControl(''),
      receiptnum12_anx1: new FormControl(''),
      ddonum12_anx1: new FormControl(''),
      date12_anx1: new FormControl(''),
      status12_anx1: new FormControl(''),
      total_tax_deposited_anx1: new FormControl('')
    })

    this.anx2Form = new FormGroup({
      employee_gid: new FormControl(''),
      assessment_gid: new FormControl(''),
      totaltax_dep1_anx2: new FormControl(''),
      bsrcode1_anx2: new FormControl(''),
      date1_anx2: new FormControl(''),
      challan1_anx2: new FormControl(''),
      status1_anx2: new FormControl(''),
      totaltax_dep2_anx2: new FormControl(''),
      bsrcode2_anx2: new FormControl(''),
      date2_anx2: new FormControl(''),
      challan2_anx2: new FormControl(''),
      status2_anx2: new FormControl(''),
      totaltax_dep3_anx2: new FormControl(''),
      bsrcode3_anx2: new FormControl(''),
      date3_anx2: new FormControl(''),
      challan3_anx2: new FormControl(''),
      status3_anx2: new FormControl(''),

      totaltax_dep4_anx2: new FormControl(''),
      bsrcode4_anx2: new FormControl(''),
      date4_anx2: new FormControl(''),
      challan4_anx2: new FormControl(''),
      status4_anx2: new FormControl(''),

      totaltax_dep5_anx2: new FormControl(''),
      bsrcode5_anx2: new FormControl(''),
      date5_anx2: new FormControl(''),
      challan5_anx2: new FormControl(''),
      status5_anx2: new FormControl(''),

      totaltax_dep6_anx2: new FormControl(''),
      bsrcode6_anx2: new FormControl(''),
      date6_anx2: new FormControl(''),
      challan6_anx2: new FormControl(''),
      status6_anx2: new FormControl(''),

      totaltax_dep7_anx2: new FormControl(''),
      bsrcode7_anx2: new FormControl(''),
      date7_anx2: new FormControl(''),
      challan7_anx2: new FormControl(''),
      status7_anx2: new FormControl(''),

      totaltax_dep8_anx2: new FormControl(''),
      bsrcode8_anx2: new FormControl(''),
      date8_anx2: new FormControl(''),
      challan8_anx2: new FormControl(''),
      status8_anx2: new FormControl(''),

      totaltax_dep9_anx2: new FormControl(''),
      bsrcode9_anx2: new FormControl(''),
      date9_anx2: new FormControl(''),
      challan9_anx2: new FormControl(''),
      status9_anx2: new FormControl(''),

      totaltax_dep10_anx2: new FormControl(''),
      bsrcode10_anx2: new FormControl(''),
      date10_anx2: new FormControl(''),
      challan10_anx2: new FormControl(''),
      status10_anx2: new FormControl(''),

      totaltax_dep11_anx2: new FormControl(''),
      bsrcode11_anx2: new FormControl(''),
      date11_anx2: new FormControl(''),
      challan11_anx2: new FormControl(''),
      status11_anx2: new FormControl(''),

      totaltax_dep12_anx2: new FormControl(''),
      bsrcode12_anx2: new FormControl(''),
      date12_anx2: new FormControl(''),
      challan12_anx2: new FormControl(''),
      status12_anx2: new FormControl(''),
      total_tax_deposited_anx2: new FormControl('')

    })
  }

  ngOnInit() {
    debugger;
    const employee_gid = this.route.snapshot.paramMap.get('employee_gidassessment_gid');
    this.employee_gid = employee_gid;

    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.employee_gid, secretKey).toString(enc.Utf8);

    const [month, year] = deencryptedParam.split('+');
    this.assessment_gid = month;
    this.employee_gid = year;

    const options: Options = {
      dateFormat: 'd-m-Y',
    };

    flatpickr('.date-picker', options);

    var api1 = 'PayMstAssessmentSummary/Getfinyeardropdown';
    this.service.get(api1).subscribe((result: any) => {
      this.finyear_list = result.Getfinyeardropdown;

    });

    this.GetPersonaldata();
    this.getincometaxsummary();
    this.GetQuatersdata();
    this.GetIncometaxinfo();
    this.GetTDSdata();
    this.Getanx1data();
    this.Getanx2data();
  }

  formatDate(dateString: string): string {
    const [year, month, day] = dateString.split('-');
    return `${day}-${month}-${year}`;
  }

  get first_name() {
    return this.personalinfoForm.get('first_name')!;
  }

  get phone() {
    return this.personalinfoForm.get('phone')!;
  }

  get email_id() {
    return this.personalinfoForm.get('email_id')!;
  }

  get postal_code() {
    return this.personalinfoForm.get('postal_code')!;
  }

  get document_type() {
    return this.incometaxForm.get('document_type')!;
  }

  get document_title() {
    return this.incometaxForm.get('document_title')!;
  }

  GetPersonaldata() {
    var api = 'PayMstAssessmentSummary/GetPersonaldata';

    let param = {
      employee_gid: this.employee_gid
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      debugger
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

  updatepersonaldetails() {
    var api = 'PayMstAssessmentSummary/UpdatePersonalInfo';
    this.service.post(api, this.personalinfoForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  getincometaxsummary() {
    var api = 'PayMstAssessmentSummary/Getincometaxsummary';
    this.service.get(api).subscribe((result: any) => {
      $('#incometax').DataTable().destroy();
      this.incometax = result.incometaxsummary_lists;
      setTimeout(() => {
        $('#incometax').DataTable();
      }, 1);
    });
  }

  submit() {
    debugger;
    const api = 'PayMstAssessmentSummary/PostIncometax'
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
    debugger;

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

  GetQuatersdata() {
    var api = 'PayMstAssessmentSummary/GetQuatersdata';

    let param = {
      employee_gid: this.employee_gid
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.quartersinfo = result;
      this.quartersinfoForm.get("q1_rpt_original_statement")?.setValue(this.quartersinfo.tdsquarter1_receiptno);
      this.quartersinfoForm.get("q1_amt_paid_credited")?.setValue(this.quartersinfo.tdsquarter1_paidcredited);
      this.quartersinfoForm.get("q1_amt_tax_deducted")?.setValue(this.quartersinfo.tdsquarter1_amount_deducted);
      this.quartersinfoForm.get("q1_amt_tax_deposited")?.setValue(this.quartersinfo.tdsquarter1_amount_deposited);
      this.quartersinfoForm.get("q2_rpt_original_statement")?.setValue(this.quartersinfo.tdsquarter2_receiptno);
      this.quartersinfoForm.get("q2_amt_paid_credited")?.setValue(this.quartersinfo.tdsquarter2_paidcredited);
      this.quartersinfoForm.get("q2_amt_tax_deducted")?.setValue(this.quartersinfo.tdsquarter2_amount_deducted);
      this.quartersinfoForm.get("q2_amt_tax_deposited")?.setValue(this.quartersinfo.tdsquarter2_amount_deposited);
      this.quartersinfoForm.get("q3_rpt_original_statement")?.setValue(this.quartersinfo.tdsquarter3_receiptno);
      this.quartersinfoForm.get("q3_amt_paid_credited")?.setValue(this.quartersinfo.tdsquarter3_paidcredited);
      this.quartersinfoForm.get("q3_amt_tax_deducted")?.setValue(this.quartersinfo.tdsquarter3_amount_deducted);
      this.quartersinfoForm.get("q3_amt_tax_deposited")?.setValue(this.quartersinfo.tdsquarter3_amount_deposited);
      this.quartersinfoForm.get("q4_rpt_original_statement")?.setValue(this.quartersinfo.tdsquarter4_receiptno);
      this.quartersinfoForm.get("q4_amt_paid_credited")?.setValue(this.quartersinfo.tdsquarter4_paidcredited);
      this.quartersinfoForm.get("q4_amt_tax_deducted")?.setValue(this.quartersinfo.tdsquarter4_amount_deducted);
      this.quartersinfoForm.get("q4_amt_tax_deposited")?.setValue(this.quartersinfo.tdsquarter4_amount_deposited);
      this.quartersinfoForm.get("total_amt_paid_credited")?.setValue(this.quartersinfo.totalamount_paidcredited);
      this.quartersinfoForm.get("total_amt_tax_deducted")?.setValue(this.quartersinfo.tdsquarter_totalamount_deducted);
      this.quartersinfoForm.get("total_amt_tax_deposited")?.setValue(this.quartersinfo.tdsquarter_totalamount_deposited);
    });
  }

  submitquartersdetails() {
    var api = 'PayMstAssessmentSummary/PostQuartersInfo';
    this.quartersinfoForm.get("employee_gid")?.setValue(this.employee_gid);
    this.quartersinfoForm.get("assessment_gid")?.setValue(this.assessment_gid);

    this.service.post(api, this.quartersinfoForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  GetIncometaxinfo() {
    debugger;
    this.incomeForm.reset();
    var api = 'PayMstAssessmentSummary/GetIncomedata';

    let param = {
      employee_gid: this.employee_gid,
      assessment_gid: this.assessment_gid
    }

    this.service.getparams(api, param).subscribe((result: any) => {

      this.responsedata = result;
      this.incomeinfo = result;

      this.incomeForm.get("employee_gid")?.setValue(this.incomeinfo.employee_gid);
      this.incomeForm.get("assessment_gid")?.setValue(this.incomeinfo.assessment_gid);
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
    var api = 'PayMstAssessmentSummary/PostIncome';
    this.incomeForm.get("employee_gid")?.setValue(this.employee_gid);
    this.incomeForm.get("assessment_gid")?.setValue(this.assessment_gid);

    this.service.post(api, this.incomeForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  GetTDSdata() {
    var api = 'PayMstAssessmentSummary/GetTDSdata';

    let param = {
      assessment_gid: this.assessment_gid,
      employee_gid: this.employee_gid
    }

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.tdsinfo = result;

      this.tdsinfoForm.get("assessment_gid")?.setValue(this.tdsinfo.assessment_gid);
      this.tdsinfoForm.get("employee_gid")?.setValue(this.tdsinfo.employee_gid);
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
      this.tdsinfoForm.get("taxpercentold")?.setValue(this.tdsinfo.taxpercentold);
      this.tdsinfoForm.get("taxpercentnew")?.setValue(this.tdsinfo.taxpercentnew);
      this.tdsinfoForm.get("tax_on_total_income")?.setValue(this.tdsinfo.tax_on_total_income);
      this.tdsinfoForm.get("educationcess")?.setValue(this.tdsinfo.educationcess);
      this.tdsinfoForm.get("tax_payable")?.setValue(this.tdsinfo.tax_payable);
      this.tdsinfoForm.get("less_relief")?.setValue(this.tdsinfo.less_relief);
      this.tdsinfoForm.get("net_tax_payable")?.setValue(this.tdsinfo.net_tax_payable);
      this.tdsinfoForm.get("less_tax_deducted_at_source")?.setValue(this.tdsinfo.less_tax_deducted_at_source);
      this.tdsinfoForm.get("balance_tax_pay_refund")?.setValue(this.tdsinfo.balance_tax_pay_refund);
    });
  }

  submittdsdetails() {
    debugger
    var api = 'PayMstAssessmentSummary/PostTDS';
    this.tdsinfoForm.get("employee_gid")?.setValue(this.employee_gid);
    this.tdsinfoForm.get("assessment_gid")?.setValue(this.assessment_gid);

    this.service.post(api, this.tdsinfoForm.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  Getanx1data() {
    var api = 'PayMstAssessmentSummary/Getanx1data';

    let param = {
      assessment_gid: this.assessment_gid,
      employee_gid: this.employee_gid
    }

    const date1_anx1 = this.anx2info.date_transfer1;
    const formatteddate_transfer1 = this.formatDate(date1_anx1);

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.anx1info = result;
      this.anx1Form.get("assessment_gid")?.setValue(this.anx1info.assessment_gid);
      this.anx1Form.get("employee_gid")?.setValue(this.anx1info.employee_gid);
      this.anx1Form.get("totaltax_dep1_anx1")?.setValue(this.anx1info.tax_deposit1);
      this.anx1Form.get("receiptnum1_anx1")?.setValue(this.anx1info.receipt_no1);
      this.anx1Form.get("ddonum1_anx1")?.setValue(this.anx1info.ddo_no1);
      this.anx1Form.get("date1_anx1")?.setValue(formatteddate_transfer1);
      this.anx1Form.get("status1_anx1")?.setValue(this.anx1info.status1);
      this.anx1Form.get("totaltax_dep2_anx1")?.setValue(this.anx1info.tax_deposit2);
      this.anx1Form.get("receiptnum2_anx1")?.setValue(this.anx1info.receipt_no2);
      this.anx1Form.get("ddonum2_anx1")?.setValue(this.anx1info.ddo_no2);
      this.anx1Form.get("date2_anx1")?.setValue(this.anx1info.date_transfer2);
      this.anx1Form.get("status2_anx1")?.setValue(this.anx1info.status2);
      this.anx1Form.get("totaltax_dep3_anx1")?.setValue(this.anx1info.tax_deposit3);
      this.anx1Form.get("receiptnum3_anx1")?.setValue(this.anx1info.receipt_no3);
      this.anx1Form.get("ddonum3_anx1")?.setValue(this.anx1info.ddo_no3);
      this.anx1Form.get("date3_anx1")?.setValue(this.anx1info.date_transfer3);
      this.anx1Form.get("status3_anx1")?.setValue(this.anx1info.status3);
      this.anx1Form.get("totaltax_dep4_anx1")?.setValue(this.anx1info.tax_deposit4);
      this.anx1Form.get("receiptnum4_anx1")?.setValue(this.anx1info.receipt_no4);
      this.anx1Form.get("ddonum4_anx1")?.setValue(this.anx1info.ddo_no4);
      this.anx1Form.get("date4_anx1")?.setValue(this.anx1info.date_transfer4);
      this.anx1Form.get("status4_anx1")?.setValue(this.anx1info.status4);
      this.anx1Form.get("totaltax_dep5_anx1")?.setValue(this.anx1info.tax_deposit5);
      this.anx1Form.get("receiptnum5_anx1")?.setValue(this.anx1info.receipt_no5);
      this.anx1Form.get("ddonum5_anx1")?.setValue(this.anx1info.ddo_no5);
      this.anx1Form.get("date5_anx1")?.setValue(this.anx1info.date_transfer5);
      this.anx1Form.get("status5_anx1")?.setValue(this.anx1info.status5);
      this.anx1Form.get("totaltax_dep6_anx1")?.setValue(this.anx1info.tax_deposit6);
      this.anx1Form.get("receiptnum6_anx1")?.setValue(this.anx1info.receipt_no6);
      this.anx1Form.get("ddonum6_anx1")?.setValue(this.anx1info.ddo_no6);
      this.anx1Form.get("date6_anx1")?.setValue(this.anx1info.date_transfer6);
      this.anx1Form.get("status6_anx1")?.setValue(this.anx1info.status6);
      this.anx1Form.get("totaltax_dep7_anx1")?.setValue(this.anx1info.tax_deposit7);
      this.anx1Form.get("receiptnum7_anx1")?.setValue(this.anx1info.receipt_no7);
      this.anx1Form.get("ddonum7_anx1")?.setValue(this.anx1info.ddo_no7);
      this.anx1Form.get("date7_anx1")?.setValue(this.anx1info.date_transfer7);
      this.anx1Form.get("status7_anx1")?.setValue(this.anx1info.status7);
      this.anx1Form.get("totaltax_dep8_anx1")?.setValue(this.anx1info.tax_deposit8);
      this.anx1Form.get("receiptnum8_anx1")?.setValue(this.anx1info.receipt_no8);
      this.anx1Form.get("ddonum8_anx1")?.setValue(this.anx1info.ddo_no8);
      this.anx1Form.get("date8_anx1")?.setValue(this.anx1info.date_transfer8);
      this.anx1Form.get("status8_anx1")?.setValue(this.anx1info.status8);
      this.anx1Form.get("totaltax_dep9_anx1")?.setValue(this.anx1info.tax_deposit9);
      this.anx1Form.get("receiptnum9_anx1")?.setValue(this.anx1info.receipt_no9);
      this.anx1Form.get("ddonum9_anx1")?.setValue(this.anx1info.ddo_no9);
      this.anx1Form.get("date9_anx1")?.setValue(this.anx1info.date_transfer9);
      this.anx1Form.get("status9_anx1")?.setValue(this.anx1info.status9);
      this.anx1Form.get("totaltax_dep10_anx1")?.setValue(this.anx1info.tax_deposit10);
      this.anx1Form.get("receiptnum10_anx1")?.setValue(this.anx1info.receipt_no10);
      this.anx1Form.get("ddonum10_anx1")?.setValue(this.anx1info.ddo_no10);
      this.anx1Form.get("date10_anx1")?.setValue(this.anx1info.date_transfer10);
      this.anx1Form.get("status10_anx1")?.setValue(this.anx1info.status10);
      this.anx1Form.get("totaltax_dep11_anx1")?.setValue(this.anx1info.tax_deposit11);
      this.anx1Form.get("receiptnum11_anx1")?.setValue(this.anx1info.receipt_no11);
      this.anx1Form.get("ddonum11_anx1")?.setValue(this.anx1info.ddo_no11);
      this.anx1Form.get("date11_anx1")?.setValue(this.anx1info.date_transfer11);
      this.anx1Form.get("status11_anx1")?.setValue(this.anx1info.status11);
      this.anx1Form.get("totaltax_dep12_anx1")?.setValue(this.anx1info.tax_deposit12);
      this.anx1Form.get("receiptnum12_anx1")?.setValue(this.anx1info.receipt_no12);
      this.anx1Form.get("ddonum12_anx1")?.setValue(this.anx1info.ddo_no12);
      this.anx1Form.get("date12_anx1")?.setValue(this.anx1info.date_transfer12);
      this.anx1Form.get("status12_anx1")?.setValue(this.anx1info.status12);
      this.anx1Form.get("total_tax_deposited_anx1")?.setValue(this.anx1info.total_taxdeposit);
    });
  }

  submitanx1details() {
    var api = 'PayMstAssessmentSummary/Postanx1';
    this.anx1Form.get("employee_gid")?.setValue(this.employee_gid);
    this.anx1Form.get("assessment_gid")?.setValue(this.assessment_gid);

    this.service.post(api, this.anx1Form.value).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  Getanx2data() {
    var api = 'PayMstAssessmentSummary/Getanx2data';

    let param = {
      assessment_gid: this.assessment_gid,
      employee_gid: this.employee_gid
    }

    const date1_anx2 = this.anx2info.date_tax1;
    const formatteddate_tax1 = this.formatDate(date1_anx2);

    this.service.getparams(api, param).subscribe((result: any) => {
      this.responsedata = result;
      this.anx2info = result;
      this.anx2Form.get("assessment_gid")?.setValue(this.anx2info.assessment_gid);
      this.anx2Form.get("employee_gid")?.setValue(this.anx2info.employee_gid);
      this.anx2Form.get("totaltax_dep1_anx2")?.setValue(this.anx2info.totaltax_deposited1);
      this.anx2Form.get("bsrcode1_anx2")?.setValue(this.anx2info.bsrcode1);
      this.anx2Form.get("date1_anx2")?.setValue(formatteddate_tax1);
      this.anx2Form.get("challan1_anx2")?.setValue(this.anx2info.challanno_tax1);
      this.anx2Form.get("status1_anx2")?.setValue(this.anx2info.status1);
      this.anx2Form.get("totaltax_dep2_anx2")?.setValue(this.anx2info.totaltax_deposited2);
      this.anx2Form.get("bsrcode2_anx2")?.setValue(this.anx2info.bsrcode2);
      this.anx2Form.get("date2_anx2")?.setValue(this.anx2info.date_tax2);
      this.anx2Form.get("challan2_anx2")?.setValue(this.anx2info.challanno_tax2);
      this.anx2Form.get("status2_anx2")?.setValue(this.anx2info.status2);
      this.anx2Form.get("totaltax_dep3_anx2")?.setValue(this.anx2info.totaltax_deposited3);
      this.anx2Form.get("bsrcode3_anx2")?.setValue(this.anx2info.bsrcode3);
      this.anx2Form.get("date3_anx2")?.setValue(this.anx2info.date_tax3);
      this.anx2Form.get("challan3_anx2")?.setValue(this.anx2info.challanno_tax3);
      this.anx2Form.get("status3_anx2")?.setValue(this.anx2info.status3);
      this.anx2Form.get("totaltax_dep4_anx2")?.setValue(this.anx2info.totaltax_deposited4);
      this.anx2Form.get("bsrcode4_anx2")?.setValue(this.anx2info.bsrcode4);
      this.anx2Form.get("date4_anx2")?.setValue(this.anx2info.date_tax4);
      this.anx2Form.get("challan4_anx2")?.setValue(this.anx2info.challanno_tax4);
      this.anx2Form.get("status4_anx2")?.setValue(this.anx2info.status4);
      this.anx2Form.get("totaltax_dep5_anx2")?.setValue(this.anx2info.totaltax_deposited5);
      this.anx2Form.get("bsrcode5_anx2")?.setValue(this.anx2info.bsrcode5);
      this.anx2Form.get("date5_anx2")?.setValue(this.anx2info.date_tax5);
      this.anx2Form.get("challan5_anx2")?.setValue(this.anx2info.challanno_tax5);
      this.anx2Form.get("status5_anx2")?.setValue(this.anx2info.status5);
      this.anx2Form.get("totaltax_dep6_anx2")?.setValue(this.anx2info.totaltax_deposited6);
      this.anx2Form.get("bsrcode6_anx2")?.setValue(this.anx2info.bsrcode6);
      this.anx2Form.get("date6_anx2")?.setValue(this.anx2info.date_tax6);
      this.anx2Form.get("challan6_anx2")?.setValue(this.anx2info.challanno_tax6);
      this.anx2Form.get("status6_anx2")?.setValue(this.anx2info.status6);
      this.anx2Form.get("totaltax_dep7_anx2")?.setValue(this.anx2info.totaltax_deposited7);
      this.anx2Form.get("bsrcode7_anx2")?.setValue(this.anx2info.bsrcode7);
      this.anx2Form.get("date7_anx2")?.setValue(this.anx2info.date_tax7);
      this.anx2Form.get("challan7_anx2")?.setValue(this.anx2info.challanno_tax7);
      this.anx2Form.get("status7_anx2")?.setValue(this.anx2info.status7);
      this.anx2Form.get("totaltax_dep8_anx2")?.setValue(this.anx2info.totaltax_deposited8);
      this.anx2Form.get("bsrcode8_anx2")?.setValue(this.anx2info.bsrcode8);
      this.anx2Form.get("date8_anx2")?.setValue(this.anx2info.date_tax8);
      this.anx2Form.get("challan8_anx2")?.setValue(this.anx2info.challanno_tax8);
      this.anx2Form.get("status8_anx2")?.setValue(this.anx2info.status8);
      this.anx2Form.get("totaltax_dep9_anx2")?.setValue(this.anx2info.totaltax_deposited9);
      this.anx2Form.get("bsrcode9_anx2")?.setValue(this.anx2info.bsrcode9);
      this.anx2Form.get("date9_anx2")?.setValue(this.anx2info.date_tax9);
      this.anx2Form.get("challan9_anx2")?.setValue(this.anx2info.challanno_tax9);
      this.anx2Form.get("status9_anx2")?.setValue(this.anx2info.status9);
      this.anx2Form.get("totaltax_dep10_anx2")?.setValue(this.anx2info.totaltax_deposited10);
      this.anx2Form.get("bsrcode10_anx2")?.setValue(this.anx2info.bsrcode10);
      this.anx2Form.get("date10_anx2")?.setValue(this.anx2info.date_tax10);
      this.anx2Form.get("challan10_anx2")?.setValue(this.anx2info.challanno_tax10);
      this.anx2Form.get("status10_anx2")?.setValue(this.anx2info.status10);
      this.anx2Form.get("totaltax_dep11_anx2")?.setValue(this.anx2info.totaltax_deposited11);
      this.anx2Form.get("bsrcode11_anx2")?.setValue(this.anx2info.bsrcode11);
      this.anx2Form.get("date11_anx2")?.setValue(this.anx2info.date_tax11);
      this.anx2Form.get("challan11_anx2")?.setValue(this.anx2info.challanno_tax11);
      this.anx2Form.get("status11_anx2")?.setValue(this.anx2info.status11);
      this.anx2Form.get("totaltax_dep12_anx2")?.setValue(this.anx2info.totaltax_deposited12);
      this.anx2Form.get("bsrcode12_anx2")?.setValue(this.anx2info.bsrcode12);
      this.anx2Form.get("date12_anx2")?.setValue(this.anx2info.date_tax12);
      this.anx2Form.get("challan12_anx2")?.setValue(this.anx2info.challanno_tax12);
      this.anx2Form.get("status12_anx2")?.setValue(this.anx2info.status12);
      this.anx2Form.get("total_tax_deposited_anx2")?.setValue(this.anx2info.total_taxdeposited);
    });
  }

  submitanx2details() {
    var api = 'PayMstAssessmentSummary/Postanx2';
    this.anx2Form.get("employee_gid")?.setValue(this.employee_gid);
    this.anx2Form.get("assessment_gid")?.setValue(this.assessment_gid);

    this.service.post(api, this.anx2Form.value).subscribe((result: any) => {
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
    this.grosstotal_income = this.incomecharge_headsalaries - this.employeeincome_total
  }

  amountpaidcalc() {
    this.total_amt_paid_credited = ((+this.q1_amt_paid_credited) + (+this.q2_amt_paid_credited) + (+this.q3_amt_paid_credited) + (+this.q4_amt_paid_credited))
  }

  amounttaxdeductioncalc() {
    this.total_amt_tax_deducted = ((+this.q1_amt_tax_deducted) + (+this.q2_amt_tax_deducted) + (+this.q3_amt_tax_deducted) + (+this.q4_amt_tax_deducted))
  }

  amounttaxdepositcalc() {
    this.total_amt_tax_deposited = ((+this.q1_amt_tax_deposited) + (+this.q2_amt_tax_deposited) + (+this.q3_amt_tax_deposited) + (+this.q4_amt_tax_deposited))
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

  aggregate4Asectiondeductiblescalc() {
    this.aggregate4Asec_deductible_total = (+this.section80C_vii_deductable_total) + (+this.section80CCC_deductable_total) + (+this.section80CCD_deductable_total) + (+this.aggregate3sec_deductable_total) + (+this.section80CCD1B_deductable_total) + (+this.other_section1_deductable) + (+this.other_section2_deductable) + (+this.other_section3_deductable) + (+this.other_section4_deductable) + (+this.other_section5_deductable);
    this.totaltaxableincome();
  }

  totaltaxableincome() {
    const grosstotal_income = parseFloat(this.grosstotal_income.toString().replace(/,/g, ''));
    const aggregate4Asec_deductible_total = this.aggregate4Asec_deductible_total || 0;
    this.total_taxable_income = grosstotal_income - aggregate4Asec_deductible_total;

    if (this.total_taxable_income < 250001) {
      this.tax_on_total_income = this.total_taxable_income;
      this.taxpercentold = undefined;
      this.taxpercentnew = undefined;
    }

    this.tdsinfoForm.get('total_taxable_income')?.setValue(this.total_taxable_income);
    this.tdsinfoForm.get('tax_on_total_income')?.setValue(this.tax_on_total_income);
  }

  taxontotalincome() {
    const totalTaxableIncome = parseFloat(this.total_taxable_income.toString().replace(/,/g, ''));

    if (isNaN(totalTaxableIncome)) {
      return;
    }

    if (totalTaxableIncome < 250001) {
      this.tax_on_total_income = totalTaxableIncome;
      this.taxpercentold = undefined;
      this.taxpercentnew = undefined;
    } else {
      if (this.tdsinfoForm.get('tax_regime')?.value === 'OLD') {
        if (totalTaxableIncome >= 250001 && totalTaxableIncome <= 300000) {
          this.taxpercentold = '5';
        }
        else if (totalTaxableIncome >= 300001 && totalTaxableIncome <= 500000) {
          this.taxpercentold = '5';
        }
        else if (totalTaxableIncome >= 500001 && totalTaxableIncome <= 1000000) {
          this.taxpercentold = '20';
        }
        else if (totalTaxableIncome >= 1000000) {
          this.taxpercentold = '30';
        }
        debugger;
        this.tdsinfoForm.get("taxpercentold")?.setValue(this.taxpercentold);
        this.taxpercentnew = undefined;

        this.tax_on_total_income = parseFloat(((totalTaxableIncome * parseFloat(this.taxpercentold)) / 100).toFixed(2));
      } else if (this.tdsinfoForm.get('tax_regime')?.value === 'NEW') {
        if (totalTaxableIncome >= 300001 && totalTaxableIncome <= 600000) {
          this.taxpercentnew = '5';
        }
        else if (totalTaxableIncome >= 600001 && totalTaxableIncome <= 900000) {
          this.taxpercentnew = '10';
        }
        else if (totalTaxableIncome >= 900001 && totalTaxableIncome <= 1200000) {
          this.taxpercentnew = '15';
        }
        else if (totalTaxableIncome >= 1200001 && totalTaxableIncome <= 1500000) {
          this.taxpercentnew = '20';
        }
        else if (totalTaxableIncome >= 1500000) {
          this.taxpercentnew = '30';
        }
        this.tdsinfoForm.get("taxpercentnew")?.setValue(this.taxpercentnew);
        this.taxpercentold = undefined;
        this.tax_on_total_income = parseFloat(((totalTaxableIncome * parseFloat(this.taxpercentnew)) / 100).toFixed(2));
      }
    }
    this.tdsinfoForm.get('tax_on_total_income')?.setValue(this.tax_on_total_income);
  }

  educationcesscalc() {
    const tax_on_total_income = this.tax_on_total_income || 0;
    const educationcesspercent = 3;
    
    this.educationcess = parseFloat(((tax_on_total_income * educationcesspercent) / 100).toFixed(2));
    this.tdsinfoForm.get('educationcess')?.setValue(this.educationcess);

    this.tax_payable = parseFloat((+tax_on_total_income + +this.educationcess).toFixed(2));
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
    this.router.navigate(['/payroll/PayMstAssessmentsummary'])
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
}
