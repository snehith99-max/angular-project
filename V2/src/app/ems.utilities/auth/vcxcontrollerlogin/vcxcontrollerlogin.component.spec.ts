import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VcxcontrollerloginComponent } from './vcxcontrollerlogin.component';

describe('VcxcontrollerloginComponent', () => {
  let component: VcxcontrollerloginComponent;
  let fixture: ComponentFixture<VcxcontrollerloginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VcxcontrollerloginComponent]
    });
    fixture = TestBed.createComponent(VcxcontrollerloginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
