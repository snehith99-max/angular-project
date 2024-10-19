import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTwhatsappanalyticsComponent } from './crm-trn-twhatsappanalytics.component';

describe('CrmTrnTwhatsappanalyticsComponent', () => {
  let component: CrmTrnTwhatsappanalyticsComponent;
  let fixture: ComponentFixture<CrmTrnTwhatsappanalyticsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTwhatsappanalyticsComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTwhatsappanalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
