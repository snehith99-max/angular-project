import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmForm22Component } from './hrm-form22.component';

describe('HrmForm22Component', () => {
  let component: HrmForm22Component;
  let fixture: ComponentFixture<HrmForm22Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HrmForm22Component]
    });
    fixture = TestBed.createComponent(HrmForm22Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
