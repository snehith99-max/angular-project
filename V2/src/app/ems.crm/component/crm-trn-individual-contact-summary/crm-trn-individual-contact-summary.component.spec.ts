import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnIndividualContactSummaryComponent } from './crm-trn-individual-contact-summary.component';

describe('CrmTrnIndividualContactSummaryComponent', () => {
  let component: CrmTrnIndividualContactSummaryComponent;
  let fixture: ComponentFixture<CrmTrnIndividualContactSummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnIndividualContactSummaryComponent]
    });
    fixture = TestBed.createComponent(CrmTrnIndividualContactSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
