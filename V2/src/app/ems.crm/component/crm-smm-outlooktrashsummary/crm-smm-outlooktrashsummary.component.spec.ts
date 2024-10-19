import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlooktrashsummaryComponent } from './crm-smm-outlooktrashsummary.component';

describe('CrmSmmOutlooktrashsummaryComponent', () => {
  let component: CrmSmmOutlooktrashsummaryComponent;
  let fixture: ComponentFixture<CrmSmmOutlooktrashsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlooktrashsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlooktrashsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
