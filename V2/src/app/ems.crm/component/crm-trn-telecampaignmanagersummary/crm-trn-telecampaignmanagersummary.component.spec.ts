import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryComponent } from './crm-trn-telecampaignmanagersummary.component';

describe('CrmTrnTelecampaignmanagersummaryComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
