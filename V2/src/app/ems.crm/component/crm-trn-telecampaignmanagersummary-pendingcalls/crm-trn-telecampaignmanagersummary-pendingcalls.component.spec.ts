import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelecampaignmanagersummaryPendingcallsComponent } from './crm-trn-telecampaignmanagersummary-pendingcalls.component';

describe('CrmTrnTelecampaignmanagersummaryPendingcallsComponent', () => {
  let component: CrmTrnTelecampaignmanagersummaryPendingcallsComponent;
  let fixture: ComponentFixture<CrmTrnTelecampaignmanagersummaryPendingcallsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelecampaignmanagersummaryPendingcallsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelecampaignmanagersummaryPendingcallsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
