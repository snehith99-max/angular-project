import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryNewComponent } from './crm-trn-telecampaignmanagersummary-new.component';

describe('CrmTrnTelecampaignmanagersummaryNewComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryNewComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryNewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
