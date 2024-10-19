import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnDepromoteaddComponent } from './hrm-trn-depromoteadd.component';

describe('HrmTrnDepromoteaddComponent', () => {
  let component: HrmTrnDepromoteaddComponent;
  let fixture: ComponentFixture<HrmTrnDepromoteaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnDepromoteaddComponent]
    });
    fixture = TestBed.createComponent(HrmTrnDepromoteaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
