import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailcampaignsendComponent } from './crm-smm-gmailcampaignsend.component';

describe('CrmSmmGmailcampaignsendComponent', () => {
  let component: CrmSmmGmailcampaignsendComponent;
  let fixture: ComponentFixture<CrmSmmGmailcampaignsendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailcampaignsendComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailcampaignsendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
