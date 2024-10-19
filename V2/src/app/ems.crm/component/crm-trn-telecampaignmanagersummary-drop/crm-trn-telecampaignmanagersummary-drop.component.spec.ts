import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryDropComponent } from './crm-trn-telecampaignmanagersummary-drop.component';

describe('CrmTrnTelecampaignmanagersummaryDropComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryDropComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryDropComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryDropComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
