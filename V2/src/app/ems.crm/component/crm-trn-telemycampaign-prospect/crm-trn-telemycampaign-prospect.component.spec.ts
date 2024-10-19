import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignProspectComponent } from './crm-trn-telemycampaign-prospect.component';

describe('CrmTrnTelemycampaignProspectComponent', () => {
  let component: CrmTrnTelemycampaignProspectComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignProspectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignProspectComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignProspectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
