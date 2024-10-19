import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignPendingComponent } from './crm-trn-telemycampaign-pending.component';

describe('CrmTrnTelemycampaignPendingComponent', () => {
  let component: CrmTrnTelemycampaignPendingComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignPendingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignPendingComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignPendingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
