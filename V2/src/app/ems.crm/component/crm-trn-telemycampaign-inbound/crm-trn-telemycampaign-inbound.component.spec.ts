import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignInboundComponent } from './crm-trn-telemycampaign-inbound.component';

describe('CrmTrnTelemycampaignInboundComponent', () => {
  let component: CrmTrnTelemycampaignInboundComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignInboundComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignInboundComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignInboundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
