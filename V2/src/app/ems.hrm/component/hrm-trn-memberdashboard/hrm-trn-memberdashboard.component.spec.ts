import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnMemberdashboardComponent } from './hrm-trn-memberdashboard.component';

describe('HrmTrnMemberdashboardComponent', () => {
  let component: HrmTrnMemberdashboardComponent;
  let fixture: ComponentFixture<HrmTrnMemberdashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnMemberdashboardComponent]
    });
    fixture = TestBed.createComponent(HrmTrnMemberdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
