import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailtrashsummaryComponent } from './crm-smm-gmailtrashsummary.component';

describe('CrmSmmGmailtrashsummaryComponent', () => {
  let component: CrmSmmGmailtrashsummaryComponent;
  let fixture: ComponentFixture<CrmSmmGmailtrashsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailtrashsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailtrashsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
