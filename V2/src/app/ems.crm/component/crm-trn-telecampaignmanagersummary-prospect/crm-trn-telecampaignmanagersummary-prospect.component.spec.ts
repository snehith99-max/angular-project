import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryProspectComponent } from './crm-trn-telecampaignmanagersummary-prospect.component';

describe('CrmTrnTelecampaignmanagersummaryProspectComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryProspectComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryProspectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryProspectComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryProspectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
