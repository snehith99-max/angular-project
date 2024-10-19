import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailcampaignsummaryComponent } from './crm-smm-gmailcampaignsummary.component';

describe('CrmSmmGmailcampaignsummaryComponent', () => {
  let component: CrmSmmGmailcampaignsummaryComponent;
  let fixture: ComponentFixture<CrmSmmGmailcampaignsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailcampaignsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailcampaignsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
