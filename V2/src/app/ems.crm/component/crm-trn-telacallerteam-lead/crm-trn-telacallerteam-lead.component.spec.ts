import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelacallerteamLeadComponent } from './crm-trn-telacallerteam-lead.component';

describe('CrmTrnTelacallerteamLeadComponent', () => {
  let component: CrmTrnTelacallerteamLeadComponent;
  let fixture: ComponentFixture<CrmTrnTelacallerteamLeadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelacallerteamLeadComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelacallerteamLeadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
