import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailcampaignsendstatusComponent } from './crm-smm-gmailcampaignsendstatus.component';

describe('CrmSmmGmailcampaignsendstatusComponent', () => {
  let component: CrmSmmGmailcampaignsendstatusComponent;
  let fixture: ComponentFixture<CrmSmmGmailcampaignsendstatusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailcampaignsendstatusComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailcampaignsendstatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
