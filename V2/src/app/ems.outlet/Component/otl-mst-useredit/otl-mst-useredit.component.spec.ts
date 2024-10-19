import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstUsereditComponent } from './otl-mst-useredit.component';

describe('OtlMstUsereditComponent', () => {
  let component: OtlMstUsereditComponent;
  let fixture: ComponentFixture<OtlMstUsereditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstUsereditComponent]
    });
    fixture = TestBed.createComponent(OtlMstUsereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
