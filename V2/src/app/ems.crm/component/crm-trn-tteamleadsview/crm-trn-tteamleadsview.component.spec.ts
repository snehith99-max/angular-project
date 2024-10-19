import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTteamleadsviewComponent } from './crm-trn-tteamleadsview.component';

describe('CrmTrnTteamleadsviewComponent', () => {
  let component: CrmTrnTteamleadsviewComponent;
  let fixture: ComponentFixture<CrmTrnTteamleadsviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTteamleadsviewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTteamleadsviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
