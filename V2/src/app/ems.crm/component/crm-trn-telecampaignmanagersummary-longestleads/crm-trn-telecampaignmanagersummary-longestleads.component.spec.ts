import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryLongestleadsComponent } from './crm-trn-telecampaignmanagersummary-longestleads.component';

describe('CrmTrnTelecampaignmanagersummaryLongestleadsComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryLongestleadsComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryLongestleadsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryLongestleadsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryLongestleadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
