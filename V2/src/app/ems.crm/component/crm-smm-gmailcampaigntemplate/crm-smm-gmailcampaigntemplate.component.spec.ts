import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailcampaigntemplateComponent } from './crm-smm-gmailcampaigntemplate.component';

describe('CrmSmmGmailcampaigntemplateComponent', () => {
  let component: CrmSmmGmailcampaigntemplateComponent;
  let fixture: ComponentFixture<CrmSmmGmailcampaigntemplateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailcampaigntemplateComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailcampaigntemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
