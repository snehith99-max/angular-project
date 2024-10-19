import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignAllComponent } from './crm-trn-telemycampaign-all.component';

describe('CrmTrnTelemycampaignAllComponent', () => {
  let component: CrmTrnTelemycampaignAllComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignAllComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignAllComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
