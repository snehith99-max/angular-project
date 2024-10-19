import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignFollowupComponent } from './crm-trn-telemycampaign-followup.component';

describe('CrmTrnTelemycampaignFollowupComponent', () => {
  let component: CrmTrnTelemycampaignFollowupComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignFollowupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignFollowupComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignFollowupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
