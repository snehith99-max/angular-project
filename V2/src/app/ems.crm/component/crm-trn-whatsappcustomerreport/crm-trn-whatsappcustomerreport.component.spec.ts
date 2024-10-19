import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnWhatsappcustomerreportComponent } from './crm-trn-whatsappcustomerreport.component';

describe('CrmTrnWhatsappcustomerreportComponent', () => {
  let component: CrmTrnWhatsappcustomerreportComponent;
  let fixture: ComponentFixture<CrmTrnWhatsappcustomerreportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnWhatsappcustomerreportComponent]
    });
    fixture = TestBed.createComponent(CrmTrnWhatsappcustomerreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
