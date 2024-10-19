import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryLapsedleadsComponent } from './crm-trn-telecampaignmanagersummary-lapsedleads.component';

describe('CrmTrnTelecampaignmanagersummaryLapsedleadsComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryLapsedleadsComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryLapsedleadsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryLapsedleadsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryLapsedleadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
