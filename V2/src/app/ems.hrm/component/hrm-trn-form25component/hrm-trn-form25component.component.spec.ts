import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmTrnForm25componentComponent } from './hrm-trn-form25component.component';

describe('HrmTrnForm25componentComponent', () => {
  let component: HrmTrnForm25componentComponent;
  let fixture: ComponentFixture<HrmTrnForm25componentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmTrnForm25componentComponent]
    });
    fixture = TestBed.createComponent(HrmTrnForm25componentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
