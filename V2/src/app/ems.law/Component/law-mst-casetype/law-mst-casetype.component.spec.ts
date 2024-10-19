import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LawMstCasetypeComponent } from './law-mst-casetype.component';

describe('LawMstCasetypeComponent', () => {
  let component: LawMstCasetypeComponent;
  let fixture: ComponentFixture<LawMstCasetypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LawMstCasetypeComponent]
    });
    fixture = TestBed.createComponent(LawMstCasetypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
