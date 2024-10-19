import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnCorporateContactSummaryComponent } from './crm-trn-corporate-contact-summary.component';

describe('CrmTrnCorporateContactSummaryComponent', () => {
  let component: CrmTrnCorporateContactSummaryComponent;
  let fixture: ComponentFixture<CrmTrnCorporateContactSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnCorporateContactSummaryComponent]
    });
    fixture = TestBed.createComponent(CrmTrnCorporateContactSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
